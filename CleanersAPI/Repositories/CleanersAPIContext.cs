using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories
{
    public class CleanersApiContext : DbContext
    {
        public CleanersApiContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().ToTable("addresses").HasKey(a => a.Id);
            modelBuilder.Entity<Address>().Property(a => a.Id).HasColumnName("addressId");
            
            modelBuilder.Entity<Customer>().ToTable("customers").HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().Property(c => c.Id).HasColumnName("customerId");
            modelBuilder.Entity<Customer>().HasOne(a => a.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Customer>().HasOne(a => a.User)
                .WithOne(u => u.Customer).HasForeignKey<Customer>(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Professional>().ToTable("professionals").HasKey(p => p.Id);
            modelBuilder.Entity<Professional>().Property(p => p.Id).HasColumnName("professionalId");
            modelBuilder.Entity<Professional>().HasOne(a => a.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Professional>().HasOne(a => a.User)
                .WithOne(u => u.Professional).HasForeignKey<Professional>(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Service>().ToTable("services").HasKey(s => s.Id);
            modelBuilder.Entity<Service>().Property(u => u.Id).HasColumnName("serviceId");
            
            modelBuilder.Entity<Expertise>().ToTable("expertises").HasKey(e => new {ProfessionId = e.ServiceId, e.ProfessionalId});
            modelBuilder.Entity<Expertise>().HasOne(e => e.Professional);
            modelBuilder.Entity<Expertise>().HasOne(e => e.Service);
            modelBuilder.Entity<Expertise>().Property(exp => exp.HourlyRate).HasColumnType("decimal(3,2)");
            
            modelBuilder.Entity<Reservation>().ToTable("reservations").HasKey(s => s.Id);
            modelBuilder.Entity<Reservation>().Property(s => s.Id).HasColumnName("reservationId");
            modelBuilder.Entity<Reservation>().HasOne(s => s.Customer).WithMany(serv => serv.Reservations);
            modelBuilder.Entity<Reservation>().HasOne(s => s.Expertise);
            modelBuilder.Entity<Reservation>().Property(r => r.TotalCost).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<User>().ToTable("users").HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("userId");
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<Role>().ToTable("roles").HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Id).HasColumnName("roleId");
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();

            modelBuilder.Entity<RoleUser>().HasKey(key => new {roleId = key.RoleId, userId = key.UserId});
            modelBuilder.Entity<RoleUser>().HasOne(key => key.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId);
            modelBuilder.Entity<RoleUser>().HasOne(key => key.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
            
            modelBuilder.Entity<Email>().ToTable("emails").HasKey(e => e.Id);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Professional> Professionals { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Expertise> Expertises { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }
        
        public DbSet<Email> Emails { get; set; }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<RoleUser> RoleUser { get; set; }
    }
}
