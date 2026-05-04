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

        public List<SaleItemRequest> Items { get; set; } = new();
    }

    public class SaleItemRequest
    {
        public List<string> SerialNumbers { get; set; } = new();
        public decimal UnitPrice { get; set; }
    }
}
