using SMT.Domain.Common;
using SMT.Domain.Enums;
using SMT.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain.Entities.Accounts
{
    public class VendorAdjustment : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long VendorId { get; set; }
        public virtual Vendor Vendor { get; set; } = new Vendor();

        public decimal Amount { get; set; } // discount received

        public VendorAdjustmentType AdjustmentType { get; set; } // "Discount", "Return"

    }
}
