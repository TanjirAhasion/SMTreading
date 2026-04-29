using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.DTO.Contacts
{
    public class CustomerDto 
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }

        public string? Phone { get; set; }
        public string? SecondaryPhone { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }

        public string? CompanyName { get; set; }

        public bool IsActive { get; set; }

    }
}
