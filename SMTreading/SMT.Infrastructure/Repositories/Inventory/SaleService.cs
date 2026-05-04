using SMT.Application.DTO.Inventory;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly ISaleItemRepository _itemRepo;
        private readonly ISaleItemSerialRepository _serialRepo;
        private readonly IProductSerialRepository _productSerialRepo;

        public SaleService(
            ISaleRepository saleRepo,
            ISaleItemRepository itemRepo,
            ISaleItemSerialRepository serialRepo,
            IProductSerialRepository productSerialRepo)
        {
            _saleRepo = saleRepo;
            _itemRepo = itemRepo;
            _serialRepo = serialRepo;
            _productSerialRepo = productSerialRepo;
        }

        public async Task<long> CreateSaleAsync(CreateSaleRequest request)
        {
            using var tx = await _saleRepo.BeginTransactionAsync();

            try
            {
                var sale = new SaleInvoice
                {
                    CustomerId = request.CustomerId,
                    SaleDate = DateTime.UtcNow,
                    Discount = request.Discount,
                    SubTotal = 0
                };

                await _saleRepo.CreateAsync(sale);

                var saleItems = new List<SaleItem>();
                var saleItemSerials = new List<SaleItemSerial>();
                decimal gross = 0;

                foreach (var reqItem in request.Items)
                {
                    var serials = await _productSerialRepo
                        .GetBySerialNumbersAsync(reqItem.SerialNumbers);

                    // 🔥 validation
                    if (serials.Count != reqItem.SerialNumbers.Count)
                        throw new Exception("Invalid serial(s)");

                    if (serials.Any(s => s.Status != ProductSerialStatus.InStock))
                        throw new Exception("Serial already sold/rented");

                    var grouped = serials.GroupBy(s => s.ProductId);

                    foreach (var group in grouped)
                    {
                        var item = new SaleItem
                        {
                            SaleId = sale.Id,
                            ProductId = group.Key,
                            Quantity = group.Count(),
                            UnitPrice = reqItem.UnitPrice
                        };

                        saleItems.Add(item);

                        gross += item.Quantity * item.UnitPrice;

                        foreach (var serial in group)
                        {
                            saleItemSerials.Add(new SaleItemSerial
                            {
                                SaleItem = item,
                                ProductSerialId = serial.Id
                            });

                            serial.Status = ProductSerialStatus.Sold;
                        }
                    }

                    await _productSerialRepo.UpdateRangeAsync(serials);
                }

                await _itemRepo.CreateRangeAsync(saleItems);
                await _serialRepo.CreateRangeAsync(saleItemSerials);

                sale.SubTotal = gross;
                await _saleRepo.UpdateAsync(sale);

                // 🔥 Ledger (call your existing ledger service)
                // Debit = SubTotal - Discount

                await tx.CommitAsync();
                return sale.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
