using Microsoft.EntityFrameworkCore;

namespace Wafer.Apis.Models
{
    public class WaferContext : DbContext
    {
        public WaferContext(DbContextOptions<WaferContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}
