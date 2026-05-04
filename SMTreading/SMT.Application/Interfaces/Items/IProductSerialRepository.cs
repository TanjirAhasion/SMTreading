using SMT.Application.DTO.Items;
using SMT.Application.Helper;
using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductSerialRepository : IBaseRepository<ProductSerial> 
    {
        Task<List<ProductSerial>> GetAllWithProductsAsync();
        Task<ProductSerial?> GetProductSerialWithSerialNumber(string serialNumber);

        Task<string> GenerateUniqueSerialAsync(string modelNumber);
        Task<bool> IsSerialNumberExistsAsync(string serialNumber);

        Task<string> GetProductSerialAsync(long id);

        Task<bool> UpdateProducSerialLinkedStatusWithImage(long id, string linkedURL);
        
        Task<PagedResult<ProductSerialDto>> GetPagedAsync(int page, int pageSize, string? search);

        Task<List<ProductSerial>> GetBySerialNumbersAsync(List<string> serials);
        Task UpdateRangeAsync(List<ProductSerial> serials);
    }
}
