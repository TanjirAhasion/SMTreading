using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class ProductImageRepository(AppDbContext db) : BaseRepository<ProductImage>(db), IProductImageRepository 
    {
        public async Task<List<ProductImage>> GetBySerialIdAsync(long serialId)
        {
            return await db.Set<ProductImage>()
                           .Where(x => x.ProductSerialId == serialId)
                           .ToListAsync();
        }
    }
}
