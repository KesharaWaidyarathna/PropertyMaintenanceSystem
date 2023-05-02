using Microsoft.EntityFrameworkCore;
using PropertyMaintenance.Modles;

namespace PropertyMaintenance.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
    
        }
        public DbSet<Maintenance> Maintenances { get; set; }
    }
}
