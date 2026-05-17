using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Inventory
{
    public class CreateRentalRequest
    {
        public long CustomerId { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime RentalInvoiceDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public string Note { get; set; } = string.Empty;
        public List<RentalItemRequest> Items { get; set; } = new();
    }

    public class SearchRentalDto
    {
        public long? CustomerId { get; set; }
        public string? SearchText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CompanyName { get; set; }
        public string? CustomerName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class RentalItemRequest
    {
        public List<string> SerialNumbers { get; set; } = new();
        public decimal RentalPrice { get; set; }
        public RateType RateType { get; set; }
    }

    public class RentalDto
    {
        public long Id { get; set; }
        public string RentalNumber { get; set; }
        public DateTime RentalDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
