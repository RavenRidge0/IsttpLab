using DealershipDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace DealershipInfrastructure
{
    public class DealershipContext : DbContext
    {
        public DealershipContext(DbContextOptions<DealershipContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
