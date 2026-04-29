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
    }
}
