using Humanizer;
using SMT.Application.DTO.Accounts;
using SMT.Application.DTO.CashManagement;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Accounts;
using SMT.Application.Interfaces.CashManagement;
using SMT.Application.Interfaces.Inventory;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Inventory;
using SMT.Domain.Enums;
using SMT.Infrastructure.Repositories.Accounts;
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
        private readonly ISaleItemProductSerialRepository _serialRepo;
        private readonly IProductSerialRepository _productSerialRepo;

        private readonly ICustomerLedgerRepository _customerLedgerRepo;

        private readonly ICustomerPaymentService _customerPaymentService;
        private readonly ICashTransactionService _cashTransactionService;

        public SaleService(
            ISaleRepository saleRepo,
            ISaleItemRepository itemRepo,
            ISaleItemProductSerialRepository serialRepo,
            IProductSerialRepository productSerialRepo,
            ICustomerLedgerRepository customerLedgerRepo,
            ICustomerPaymentService customerPaymentService,
            ICashTransactionService cashTransactionService)
        {
            _saleRepo = saleRepo;
            _itemRepo = itemRepo;
            _serialRepo = serialRepo;
            _productSerialRepo = productSerialRepo;
            _customerLedgerRepo = customerLedgerRepo;
            _customerPaymentService = customerPaymentService;
            _cashTransactionService = cashTransactionService;
        }

        public async Task<long> CreateSaleAsync(CreateSaleRequest request)
        {
            using var tx = await _saleRepo.BeginTransactionAsync();

            try
            {
                var sale = new SaleInvoice
                {
                    CustomerId = request.CustomerId,
                    InvoiceNumber = $"SAL-{DateTime.UtcNow.Ticks}",
                    SaleDate = request.SalesInvoiceDate,
                    Discount = request.Discount,
                    SubTotal = 0,
                    IsPaid = false
                };

                await _saleRepo.CreateAsync(sale);

                var saleItems = new List<SaleItem>();
                var saleItemSerials = new List<SaleItemProductSerial>();
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
                            saleItemSerials.Add(new SaleItemProductSerial
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

                // 🔥 CALCULATE TOTALS
                var netTotal = sale.SubTotal - sale.Discount;
                var paid = request.PaidAmount;
                var due = netTotal - paid;

                // Debit = SubTotal - Discount
                // 7. Customer Ledger (Debit)
                await _customerLedgerRepo.CreateCustomerLedger(new CustomerLedgerDto
                {
                    CustomerId = request.CustomerId,
                    SourceType = CustomerLedgerSourceType.Sale,
                    SourceId = sale.Id,
                    Credit = 0,
                    Debit = netTotal,
                    Description = $"Payment for Sale #{sale.InvoiceNumber}"
                });

                // 8. Payment (if any)
                if (request.PaidAmount > 0)
                {
                    await _cashTransactionService.CreateAsync(new CashTransactionDto
                    {
                        CashAccountId = request.CashAccountId,
                        Amount = request.PaidAmount,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = (int)TransactionType.CashIn,
                        SourceType = (int)TransactionSource.SalePayment,
                        ReferenceId = sale.Id,
                        Note = $"Payment for Sale #{sale.Id}"
                    });

                    var paymentId = await _customerPaymentService.CreateAsync(new CustomerPaymentDto
                    {
                        CustomerId = request.CustomerId,
                        SaleId = sale.Id,
                        CustomerPaymentType = (int)CustomerPaymentType.Sale,
                        Amount = request.PaidAmount,
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = (int)PaymentMethodEnum.Cash
                    }, sale.InvoiceNumber);
                }

                await tx.CommitAsync();
                return sale.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<SalesInvoiceDto> GetInvoiceByIdAsync(long id)
        {
            return await _saleRepo.GetInvoiceByIdAsync(id);
        }

        public async Task<PagedResult<SalesDto>> GetPagedAsync(SearchSalesDto searchSalesDto)
        {
            return await _saleRepo.GetPagedAsync(searchSalesDto);
        }
    }
}
