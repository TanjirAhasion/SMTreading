using SMT.Application.Interfaces.Inventory.Rental;
using SMT.Domain.Entities.Inventory.Rental;
using SMT.Infrastructure.context;
using SMT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories.Inventory.Rental
{
    public class RentalItemSerialRepository(AppDbContext db) : BaseRepository<RentalItemProductSerial>(db), IRentalItemSerialRepository
    {
    }
}
