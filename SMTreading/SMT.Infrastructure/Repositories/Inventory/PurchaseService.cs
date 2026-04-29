using Microsoft.EntityFrameworkCore;
using SMT.Application.DTO.Inventory;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Items;
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

        private readonly IProductRepository _productRepo;
        private readonly IProductSerialRepository _productSerialRepo;

        public PurchaseService(
            IPurchaseRepository purchaseRepo,
            IPurchaseItemRepository purchaseItemRepo,
            IPurchaseItemProductSerialRepository purchaseItemProductSerialRepo,
            IProductRepository productRepo,
            IProductSerialRepository productSerialRepo)
        {
            _purchaseRepo = purchaseRepo;
            _purchaseItemRepo = purchaseItemRepo;
            _purchaseItemProductSerialRepo = purchaseItemProductSerialRepo;
            _productRepo = productRepo;
            _productSerialRepo = productSerialRepo;

        }

        public async Task<long> CreatePurchaseAsync(CreatePurchaseRequest request)
        {
            // 1. Create Purchase
            var purchase = new Purchase
            {
                VendorId = request.VendorId,
                PurchaseDate = DateTime.UtcNow,
                SubTotal = request.SubTotal,
                Discount = request.Discount,
                NetTotal = request.SubTotal - request.Discount,
                PaidAmount = request.PaidAmount,
                DueAmount = (request.SubTotal - request.Discount) - request.PaidAmount
            };

            await _purchaseRepo.CreateAsync(purchase);

            decimal grossTotal = 0;

            foreach (var item in request.Items)
            {
                // 2. Validate product
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product not found: {item.ProductId}");

                // 3. Create Purchase Item
                var purchaseItem = new PurchaseItem
                {
                    PurchaseId = purchase.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    DiscountAllocated = item.Discount,
                    TotalCost = item.Quantity * item.UnitCost
                };

                await _purchaseItemRepo.CreateAsync(purchaseItem);

                grossTotal += purchaseItem.TotalCost;

                string batchNo = $"B-{DateTime.UtcNow:yyyyMMddHHmmss}";

                // 4. Generate Serials for each unit
                for (int i = 0; i < item.Quantity; i++)
                {
                    var serial = await GenerateUniqueSerialAsync(product.Model);

                    var productSerial = new ProductSerial
                    {
                        ProductId = product.Id,
                        Product = product,
                        SerialNumber = serial,
                        PurchaseCost = item.UnitCost,
                        Status = ProductSerialStatus.InStock,
                        BatchNumber = batchNo,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _productSerialRepo.CreateAsync(productSerial);

                    // 5. Mapping (PurchaseItem ↔ Serial)
                    var mapping = new PurchaseItemProductSerial
                    {
                        PurchaseItemId = purchaseItem.Id,
                        ProductSerialId = productSerial.Id,
                        Status = ProductSerialStatus.InStock,
                    };

                    await _purchaseItemProductSerialRepo.CreateAsync(mapping);
                }
            }

            // 6. Apply Discount
            purchase.SubTotal = grossTotal - purchase.Discount;

            await _purchaseRepo.UpdateAsync(purchase);

            return purchase.Id;
        }

        private async Task<string> GenerateUniqueSerialAsync(string modelNumber)
        {
            string serial = await _productSerialRepo.GenerateUniqueSerialAsync(modelNumber);
            return serial;
        }
    }
}
