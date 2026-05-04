using SMT.Application.DTO.Accounts;
using SMT.Application.Interfaces.Accounts;
using SMT.Domain.Entities.Accounts;
using SMT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Accounts
{
    public class VendorPaymentService : IVendorPaymentService
    {
        private readonly IVendorPaymentRepository _repo;

        public VendorPaymentService(IVendorPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<long> CreateAsync(VendorPaymentDto dto)
        {
            var entity = new VendorPayment
            {
                VendorId = dto.VendorId,
                PurchaseId = dto.PurchaseId,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate,
                PaymentMethod = (PaymentMethodEnum)dto.PaymentMethod
            };

            await _repo.CreateAsync(entity);
            return entity.Id;
        }

        public async Task<List<VendorPaymentDto>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return result.Select(x => new VendorPaymentDto
            {
                Id = x.Id,
                VendorId = x.VendorId,
                PurchaseId = x.PurchaseId,
                Amount = x.Amount,
                PaymentDate = x.PaymentDate,
                PaymentMethod = (int)x.PaymentMethod
            }).ToList();
        }

        public async Task<VendorPaymentDto?> GetByIdAsync(long id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null) return null;
            
            return new VendorPaymentDto
            {
                Id = result.Id,
                VendorId = result.VendorId,
                PurchaseId = result.PurchaseId,
                Amount = result.Amount,
                PaymentDate = result.PaymentDate,
                PaymentMethod = (int)result.PaymentMethod
            };
        }
    }
}
