using SMT.Application.DTO.Accounts;
using SMT.Application.DTO.Inventory;
using SMT.Application.Helper;
using SMT.Application.Interfaces.Accounts;
using SMT.Application.Interfaces.Inventory.Rental;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Entities.Inventory.Rental;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory.Rental
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepo;
        private readonly IRentalItemRepository _itemRepo;
        private readonly IRentalItemSerialRepository _serialRepo;
        private readonly IProductSerialRepository _productSerialRepo;

        private readonly ICustomerLedgerRepository _customerLedgerRepo;
        private readonly ICustomerPaymentRepository _customerPaymentRepo;
        public RentalService(IRentalRepository rentalRepo, IRentalItemRepository itemRepo, IRentalItemSerialRepository serialRepo, IProductSerialRepository productSerialRepo, ICustomerLedgerRepository customerLedgerRepo, ICustomerPaymentRepository customerPaymentRepo)
        {
            _rentalRepo = rentalRepo;
            _itemRepo = itemRepo;
            _serialRepo = serialRepo;
            _productSerialRepo = productSerialRepo;
            _customerLedgerRepo = customerLedgerRepo;
            _customerPaymentRepo = customerPaymentRepo;
        }
        public async Task<long> CreateRentalAsync(CreateRentalRequest request)
        {
            using var tx = await _rentalRepo.BeginTransactionAsync();

            try
            {
                //var rental = new RentalInvoice
                //{
                //    CustomerId = request.CustomerId,
                //    RentalNumber = $"RENT-{DateTime.UtcNow.Ticks}",
                //    StartDate = request.RentalInvoiceDate,
                //    Note = request.Note,
                //    Discount = request.Discount,
                //    RentPrice = 0,
                //    Deposit = 0
                //};

                //await _rentalRepo.CreateAsync(rental);

                //var rentalItems = new List<RentalInvoiceItem>();
                //var rentalItemSerials = new List<RentalItemProductSerial>();
                //decimal gross = 0;

                //foreach (var reqItem in request.Items)
                //{
                //    var serials = await _productSerialRepo
                //        .GetBySerialNumbersAsync(reqItem.SerialNumbers);

                //    // 🔥 validation
                //    if (serials.Count != reqItem.SerialNumbers.Count)
                //        throw new Exception("Invalid serial(s)");

                //    if (serials.Any(s => s.Status != ProductSerialStatus.InStock))
                //        throw new Exception("Serial already sold/rented");

                //    var grouped = serials.GroupBy(s => s.ProductId);

                //    foreach (var group in grouped)
                //    {
                //        var item = new RentalInvoiceItem
                //        {
                //            RentalId = rental.Id,
                //            ProductId = group.Key,
                //            Quantity = group.Count(),
                //            Rate = reqItem.RentalPrice,
                //            RateType = reqItem.RateType,
                //            //Quantity= group.Count()??0
                //        };

                //        rentalItems.Add(item);

                //        gross += item.Quantity * item.Rate;

                //        foreach (var serial in group)
                //        {
                //            rentalItemSerials.Add(new RentalItemProductSerial
                //            {
                //                RentalItem = item,
                //                ProductSerialId = serial.Id,
                //                Status= ProductSerialStatus.InRent,
                //            });

                //            serial.Status = ProductSerialStatus.InRent;
                //        }
                //    }

                //    await _productSerialRepo.UpdateRangeAsync(serials);
                //}

                //await _itemRepo.CreateRangeAsync(rentalItems);
                //await _serialRepo.CreateRangeAsync(rentalItemSerials);

                //rental.RentPrice = gross;
                //await _rentalRepo.UpdateAsync(rental);

                //// 🔥 CALCULATE TOTALS
                //var netTotal = rental.RentPrice - rental.Discount;
                //var paid = request.PaidAmount;
                //var due = netTotal - paid;

                //// Debit = SubTotal - Discount
                //// 7. Customer Ledger (Debit)
                //await _customerLedgerRepo.CreateCustomerLedger(new CustomerLedgerDto
                //{
                //    CustomerId = request.CustomerId,
                //    SourceType = CustomerLedgerSourceType.Rental,
                //    SourceId = rental.Id,
                //    Credit = 0,
                //    Debit = netTotal
                //});

                //// 8. Payment (if any)
                //if (request.PaidAmount > 0)
                //{
                //    var paymentId = await _customerPaymentRepo.CreateAsync(new CustomerPayment
                //    {
                //        CustomerId = request.CustomerId,
                //        RentalId = rental.Id,
                //        CustomerPaymentType = CustomerPaymentType.Rental,
                //        Amount = request.PaidAmount,
                //        PaymentDate = DateTime.UtcNow,
                //        PaymentMethod = PaymentMethodEnum.Cash
                //    });

                //    await _customerLedgerRepo.CreateCustomerLedger(new CustomerLedgerDto
                //    {
                //        CustomerId = request.CustomerId,
                //        SourceType = CustomerLedgerSourceType.Payment,
                //        SourceId = paymentId.Id,
                //        Credit = request.PaidAmount,
                //        Debit = 0
                //    });
                //}

                //await tx.CommitAsync();
                //return rental.Id;
                return 0;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<RentalDto>> GetPagedAsync(SearchRentalDto searchRentalDto)
        {
            return await _rentalRepo.GetPagedAsync(searchRentalDto);
        }
    }
}
