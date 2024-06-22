using HomeManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeManagerApi.Data
{
    public class HomeManagerContext : DbContext
    {
        public HomeManagerContext(DbContextOptions<HomeManagerContext> options):base(options)
        {
            
        }

        public DbSet<Categoria> Categorias { get; set; }

    }
}
