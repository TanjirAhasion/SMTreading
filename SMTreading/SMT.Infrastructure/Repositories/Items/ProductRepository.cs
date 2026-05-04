using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductRepository(AppDbContext db) : BaseRepository<Product>(db), IProductRepository
    {
        public async Task<List<Product>> GetAllWithBrandsAsync()
        {
            return await db.Products
                .Include(p => p.Brand)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByIdsAsync(List<long> productIds)
        {
            return await db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
