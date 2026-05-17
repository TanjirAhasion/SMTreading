using SMT.Application.DTO.Accounts;
using SMT.Application.Interfaces.Accounts;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Accounts
{
    public class CustomerPaymentService : ICustomerPaymentService
    {
        private readonly ICustomerPaymentRepository _customerPaymentRepo;
        private readonly ICustomerLedgerRepository _customerLedgerRepo;
        public CustomerPaymentService(ICustomerPaymentRepository customerPaymentRepository, ICustomerLedgerRepository customerLedgerRepository)
        {
            _customerPaymentRepo = customerPaymentRepository;
            _customerLedgerRepo = customerLedgerRepository;
        }

        public async Task<long> CreateAsync(CustomerPaymentDto dto, string? invoiceNumber)
        {
            var payment = await _customerPaymentRepo.CreateAsync(
                            new CustomerPayment
                            {
                                CustomerId = dto.CustomerId,
                                SaleId = dto.SaleId,
                                RentalId = dto.RentalId,
                                CustomerPaymentType = (CustomerPaymentType)dto.CustomerPaymentType,
                                Amount = dto.Amount,
                                PaymentDate = dto.PaymentDate,
                                PaymentMethod = (PaymentMethodEnum)dto.PaymentMethod
                            });


            var description = payment.CustomerPaymentType == CustomerPaymentType.Sale
                ? $"Payment for Sale #{invoiceNumber ?? dto.SaleId.ToString()}"
                : $"Payment for Rental #{invoiceNumber ?? dto.RentalId.ToString()}";

            await _customerLedgerRepo.CreateCustomerLedger(
                new CustomerLedgerDto
                {
                    CustomerId = dto.CustomerId,
                    SourceType = payment.CustomerPaymentType == CustomerPaymentType.Sale ? CustomerLedgerSourceType.Sale : CustomerLedgerSourceType.Rental,
                    SourceId = payment.Id,

                    Credit = dto.Amount,
                    Debit = 0,

                    Description = description
                });

            return payment.Id;
        }
    }
}
