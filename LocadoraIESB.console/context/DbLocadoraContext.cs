using LocadoraIESB.console.models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraIESB.console.context
{
    public class DbLocadoraContext : DbContext
    {
        public DbSet<Carro> Carros { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Locacao> Locacoes { get; set; }
        
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasAlternateKey(c => c.Cpf);

            modelBuilder.Entity<Carro>()
                .HasAlternateKey(c => c.Placa);

        }
    }
}