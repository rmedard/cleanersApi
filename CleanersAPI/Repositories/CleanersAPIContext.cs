using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories
{
    public class CleanersApiContext : DbContext
    {
        public CleanersApiContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().ToTable("addresses");
            modelBuilder.Entity<Customer>().ToTable("customers");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Expertise>().ToTable("expertises").HasKey(e => new {e.ProfessionId, e.ProfessionalId});
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Professional> Professionals { get; set; }

        public DbSet<Profession> Professions { get; set; }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Expertise> Expertises { get; set; }
    }
}
