using SMT.Application.Interfaces.Items;
using SMT.Domain.Entities.Items;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMT.Infrastructure.Repositories.Items
{
    public class BrandRepository (AppDbContext db): BaseRepository<Brand>(db), IBrandRepository
    {
        
    }
}
