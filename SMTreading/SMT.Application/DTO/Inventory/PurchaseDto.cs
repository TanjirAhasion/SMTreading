using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.Inventory
{
    public class CreatePurchaseDto
    {
        public long VendorId { get; set; }

        public decimal Discount { get; set; } // invoice level

        public decimal PaidAmount { get; set; }

        public List<PurchaseItemDto> Items { get; set; } = new();
    }

    public class PurchaseItemDto
    {
        public long ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitCost { get; set; }
    }

    public class CreatePurchaseRequest
    {
        public long VendorId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }

        public decimal PaidAmount { get; set; }
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
