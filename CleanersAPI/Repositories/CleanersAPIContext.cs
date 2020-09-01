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
            modelBuilder.Entity<Customer>().HasOne(a => a.User)
                .WithOne(u => u.Customer).HasForeignKey<Customer>(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Professional>().ToTable("professionals");
            modelBuilder.Entity<Professional>().HasOne(a => a.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Professional>().HasOne(a => a.User)
                .WithOne(u => u.Professional).HasForeignKey<Professional>(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Service>().ToTable("professions");
            
            modelBuilder.Entity<User>().ToTable("users").HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().ToTable("roles").HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<RoleUser>().HasKey(key => new {key.roleId, key.userId});
            modelBuilder.Entity<RoleUser>().HasOne(key => key.role).WithMany(r => r.Users).HasForeignKey(r => r.roleId);
            modelBuilder.Entity<RoleUser>().HasOne(key => key.user).WithMany(u => u.Roles).HasForeignKey(u => u.userId);
            
            modelBuilder.Entity<Reservation>().ToTable("services");
            modelBuilder.Entity<Reservation>().HasOne(s => s.Customer).WithMany(serv => serv.Orders);
            modelBuilder.Entity<Reservation>().HasOne(s => s.Expertise);
            modelBuilder.Entity<Reservation>().Property(serv => serv.TotalCost).HasColumnType("decimal(5,2)");
            
            modelBuilder.Entity<Expertise>().ToTable("professionalService").HasKey(e => new {ProfessionId = e.ServiceId, e.ProfessionalId});
            modelBuilder.Entity<Expertise>().Property(exp => exp.HourlyRate).HasColumnType("decimal(3,2)");

            modelBuilder.Entity<Email>().ToTable("emails");
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Professional> Professionals { get; set; }

        public DbSet<Service> Professions { get; set; }

        public DbSet<Expertise> Expertises { get; set; }
        
        public DbSet<Reservation> Services { get; set; }
        
        public DbSet<Email> Emails { get; set; }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<RoleUser> RoleUsers { get; set; }
    }
}
