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
            modelBuilder.Entity<Customer>().HasOne(a => a.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>().HasOne(a => a.User).WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Professional>().ToTable("professionals");
            modelBuilder.Entity<Professional>().HasOne(a => a.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
//            modelBuilder.Entity<Professional>().HasOne(a => a.User).WithOne().OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Profession>().ToTable("professions");
            
            modelBuilder.Entity<User>().ToTable("users");
            
            modelBuilder.Entity<Service>().ToTable("services");
            
            modelBuilder.Entity<Expertise>().ToTable("expertises").HasKey(e => new {e.ProfessionId, e.ProfessionalId});
            modelBuilder.Entity<Expertise>().Property(p => p.UnitPrice).HasColumnType("decimal(10,2)");
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Professional> Professionals { get; set; }

        public DbSet<Profession> Professions { get; set; }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Expertise> Expertises { get; set; }
        
        public DbSet<Service> Services { get; set; }
    }
}
