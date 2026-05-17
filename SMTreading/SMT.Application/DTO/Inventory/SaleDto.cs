using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Inventory
{
    public class CreateSaleRequest
    {
        public long CustomerId { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime SalesInvoiceDate { get; set; }

        public long CashAccountId { get; set; }
        public List<SaleItemRequest> Items { get; set; } = new();
    }

    public class SearchSalesDto
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

    public class SaleItemRequest
    {
        public List<string> SerialNumbers { get; set; } = new();
        public decimal UnitPrice { get; set; }
    }

    public class SalesDto
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public bool IsPaid { get; set; }
        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
    }

    public class SalesInvoiceDto
    {
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public bool IsPaid { get; set; }
        public string CustomerFullName { get; set; }
        public string CompanyName { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public List<SalesInvoiceItemDto> InvoiceItems { get; set; } = new List<SalesInvoiceItemDto>();
    }

    public class SalesInvoiceItemDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public List<string> SerialNumbers { get; set; } = new();
    }
}
