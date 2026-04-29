using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Infrastructure.Common;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductSerialRepository(AppDbContext db) :BaseRepository<ProductSerial>(db), IProductSerialRepository
    {
        public async Task<string> GenerateUniqueSerialAsync(string modelNumber)
        {
            string serial;

            do
            {
                serial = GeneratedProductSerial.Generate(modelNumber);
            }
            while (await IsSerialNumberExistsAsync(serial));

            return serial;
        }

        public async Task<List<ProductSerial>> GetAllWithProductsAsync()
        {
            return await db.ProductSerials
                .Include(p => p.Product)
                //.ThenInclude(p => p.Brand)
                .ToListAsync();
        }

        public async Task<ProductSerial?> GetProductSerialWithSerialNumber(string serialNumber)
        {
            return await db.ProductSerials
                .Where(ps => ps.SerialNumber == serialNumber)
                .FirstOrDefaultAsync();
        }

        public Task<bool> IsSerialNumberExistsAsync(string serialNumber)
        {
           return db.ProductSerials.AnyAsync(ps => ps.SerialNumber == serialNumber);
        }

     
    }
}
