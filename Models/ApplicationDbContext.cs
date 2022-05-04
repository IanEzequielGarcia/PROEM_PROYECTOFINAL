
using Microsoft.EntityFrameworkCore;
using PROEM_PROYECTOFINALMVC.Models;

namespace EJERCICIO_FINAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Facturas> Factura { get; set; }
        public DbSet<Pagos> Pago { get; set; }
        public DbSet<Items> Item { get; set; }
    }
}
