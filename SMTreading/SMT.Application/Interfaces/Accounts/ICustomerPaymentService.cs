using SMT.Application.DTO.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces.Accounts
{
    public interface ICustomerPaymentService
    {
        Task<long> CreateAsync(CustomerPaymentDto dto, string? invoiceNumber);
    }
}
