using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.DTO.CashManagement
{
    public class ExpenseCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
