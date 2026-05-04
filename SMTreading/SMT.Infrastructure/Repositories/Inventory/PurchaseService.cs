using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Accounts;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Accounts;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Entities.Items;
using SMT.Domain.Enums;
using SMT.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepo;
        private readonly IPurchaseItemRepository _purchaseItemRepo;
        private readonly IPurchaseItemProductSerialRepository _purchaseItemProductSerialRepo;

        private readonly IVendorPaymentService _vendorPaymentService;
        private readonly IVendorLedgerRepository _vendorLedgerRepo;

        private readonly IProductRepository _productRepo;
        private readonly IProductSerialRepository _productSerialRepo;

        public PurchaseService(
            IPurchaseRepository purchaseRepo,
            IPurchaseItemRepository purchaseItemRepo,
            IPurchaseItemProductSerialRepository purchaseItemProductSerialRepo,
            IVendorPaymentService vendorPaymentService,
            IVendorLedgerRepository vendorLedgerRepo,
            IProductRepository productRepo,
            IProductSerialRepository productSerialRepo)
        {
            _purchaseRepo = purchaseRepo;
            _purchaseItemRepo = purchaseItemRepo;
            _purchaseItemProductSerialRepo = purchaseItemProductSerialRepo;
            _vendorPaymentService = vendorPaymentService;
            _vendorLedgerRepo = vendorLedgerRepo;
            _productRepo = productRepo;
            _productSerialRepo = productSerialRepo;

        }

        public async Task<long> CreatePurchaseAsync(CreatePurchaseRequest request)
        {
            using var transaction = await _purchaseRepo.BeginTransactionAsync();

            try
            {
                // 1. Create Purchase
                var purchase = new Purchase
                {
                    VendorId = request.VendorId,
                    PurchaseNumber = $"PUR-{DateTime.UtcNow.Ticks}",
                    PurchaseDate = DateTime.UtcNow,
                    Discount = request.Discount,
                    SubTotal = 0,
                    IsPaid = false
                };

                await _purchaseRepo.CreateAsync(purchase);
                //await _purchaseRepo.SaveChangesAsync(); // 🔥 ensure purchase.Id exists

                decimal grossTotal = 0;

                // 2. Load all required products in ONE query (optimized)
                var productIds = request.Items.Select(x => x.ProductId).Distinct().ToList();
                var products = await _productRepo.GetByIdsAsync(productIds);
                var productDict = products.ToDictionary(x => x.Id);

                var purchaseItems = new List<PurchaseItem>();

                // 3. Create Purchase Items
                foreach (var item in request.Items)
                {
                    if (!productDict.ContainsKey(item.ProductId))
                        throw new Exception($"Product not found: {item.ProductId}");

                    var purchaseItem = new PurchaseItem
                    {
                        PurchaseId = purchase.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitCost = item.UnitCost,
                        DiscountAllocated = item.Discount,
                    };

                    purchaseItems.Add(purchaseItem);

                    grossTotal += item.Quantity * item.UnitCost;
                }

                await _purchaseItemRepo.CreateRangeAsync(purchaseItems);
                //await _purchaseItemRepo.SaveChangesAsync(); // 🔥 CRITICAL (IDs generated)

                // 4. Generate Product Serials
                var productSerials = new List<ProductSerial>();
                var batchNo = $"B-{Guid.NewGuid().ToString()[..8].ToUpper()}";

                foreach (var item in purchaseItems)
                {
                    var product = productDict[item.ProductId];

                    for (int i = 0; i < item.Quantity; i++)
                    {
                        var serial = await GenerateUniqueSerialAsync(product.Model);

                        productSerials.Add(new ProductSerial
                        {
                            ProductId = product.Id,
                            SerialNumber = serial,
                            PurchaseCost = item.UnitCost,
                            Status = ProductSerialStatus.InStock,
                            BatchNumber = batchNo,
                            PurchaseDate = request.PurchaseDate,
                            CreatedAt = DateTime.UtcNow,
                            Product = product
                        });
                    }
                }

                await _productSerialRepo.CreateRangeAsync(productSerials);
                //await _productSerialRepo.SaveChangesAsync(); // 🔥 CRITICAL (IDs generated)

                // 5. Create Mapping (SAFE: IDs now exist)
                var mappings = new List<PurchaseItemProductSerial>();
                int serialIndex = 0;

                foreach (var item in purchaseItems)
                {
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        mappings.Add(new PurchaseItemProductSerial
                        {
                            PurchaseItemId = item.Id,
                            ProductSerialId = productSerials[serialIndex].Id,
                            Status = ProductSerialStatus.InStock,
                            PurchaseDate = request.PurchaseDate,
                        });

                        serialIndex++;
                    }
                }

                await _purchaseItemProductSerialRepo.CreateRangeAsync(mappings);
                //await _purchaseItemProductSerialRepo.SaveChangesAsync();

                // 6. Update totals
                purchase.SubTotal = grossTotal;
                if (purchase.SubTotal - purchase.Discount == request.PaidAmount)
                {
                    purchase.IsPaid = true;
                }
                await _purchaseRepo.UpdateSubTotalAsync(purchase.Id, purchase.SubTotal);

                var netTotal = purchase.SubTotal - purchase.Discount;

                // 7. Vendor Ledger (Debit)
                await _vendorLedgerRepo.CreateVendorLedger(new VendorLedgerDto
                {
                    VendorId = request.VendorId,
                    SourceType = VendorLedgerSourceType.Purchase,
                    SourceId = purchase.Id,
                    Credit = 0,
                    Debit = netTotal
                });

                // 8. Payment (if any)
                if (request.PaidAmount > 0)
                {
                    var paymentId = await _vendorPaymentService.CreateAsync(new VendorPaymentDto
                    {
                        VendorId = request.VendorId,
                        PurchaseId = purchase.Id,
                        Amount = request.PaidAmount,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = (int)PaymentMethodEnum.Cash
                    });

                    await _vendorLedgerRepo.CreateVendorLedger(new VendorLedgerDto
                    {
                        VendorId = request.VendorId,
                        SourceType = VendorLedgerSourceType.Purchase,
                        SourceId = paymentId,
                        Credit = request.PaidAmount,
                        Debit = 0
                    });
                }

                await transaction.CommitAsync();

                return purchase.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<PurchaseDto>> GetPagedAsync(SearchPurchaseDto searchPurchaseDto)
        {
            return await _purchaseRepo.GetPagedAsync(searchPurchaseDto);
        }

        private async Task<string> GenerateUniqueSerialAsync(string modelNumber)
        {
            string serial = await _productSerialRepo.GenerateUniqueSerialAsync(modelNumber);
            return serial;
        }
    }
}
