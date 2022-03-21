using LocadoraIESB.console.models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraIESB.console.context
{
    public class DbLocadoraContext : DbContext
    {
        public DbLocadoraContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbLocadoraContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=DbLocadora;User=sasa;password=test@123");
            }
        }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        
    }
}