using SMT.Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Application.Interfaces.Items
{
    public interface IProductImageRepository : IBaseRepository<ProductImage>
    {
        Task<List<ProductImage>> GetBySerialIdAsync(long serialId);
    }
}
