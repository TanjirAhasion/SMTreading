using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Inventory
{
    public class RentalContractDto
    {
        public long Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal SecurityDeposit { get; set; }
        public int BillingCycle { get; set; }
        public string? Note { get; set; }
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public int Status { get; set; }
        public DateTime NextBillingDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
    public class SearchRentalContractDto
    {
        public long? CustomerId { get; set; }
        public string? SearchText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CompanyName { get; set; }
        public string? CustomerName { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class CreateRentalContractRequest
    {
        public long CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int BillingCycle { get; set; }

        public string PaymentMethod { get; set; }

        public decimal SecurityDeposit { get; set; }

        public string? Note { get; set; }

        public long CashAccountId { get; set; }

        public List<CreateRentalContractItemRequest> Items { get; set; }
            = new();
    }

    public class CreateRentalContractItemRequest
    {
        public List<string> SerialNumbers { get; set; }

        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Rent { get; set; }
    }

    public class RentalContractInvoiceDto
    {
        public long Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal SecurityDeposit { get; set; }
        public int BillingCycle { get; set; }
        public string? Note { get; set; }
        public string CustomerFullName { get; set; }
        public string CompanyName { get; set; }
        public int Status { get; set; }
        public DateTime NextBillingDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public List<RentalContractInvoiceItemDto> InvoiceItems { get; set; }
            = new List<RentalContractInvoiceItemDto>();
    }

    public class RentalContractInvoiceItemDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public List<string> SerialNumbers { get; set; }
        public decimal TotalRent { get; set; }
        public decimal UnitRent { get; set; }
    }
}
