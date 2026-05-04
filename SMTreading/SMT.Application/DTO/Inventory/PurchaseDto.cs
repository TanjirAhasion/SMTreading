using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Inventory
{
    public class SearchPurchaseDto
    {
        public long? VendorId { get; set; }
        public string? SearchText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CompanyName { get; set; }
        public string? VendorName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class PurchaseDto
    {
        public long Id { get; set; }
        public string PurchaseNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public bool IsPaid { get; set; }
        public long VendorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }

    public class CreatePurchaseRequest
    {
        public long VendorId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<CreatePurchaseItemRequest> Items { get; set; }
    }

    public class CreatePurchaseItemRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Discount { get; set; }
        public List<string> ProductSerialNumber { get; set; }
    }
}
