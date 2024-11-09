using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Database;

public class DatabaseRepository : DbContext
{
    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<WeaponType> WeaponTypes { get; set; }

    public DatabaseRepository(DbContextOptions<DatabaseRepository> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(user => user.UserRole)
            .WithMany(userRole => userRole.Users)
            .HasForeignKey(user => user.UserRoleId);

        modelBuilder.Entity<Weapon>()
            .HasOne(weapon => weapon.WeaponType)
            .WithMany(weaponType => weaponType.Weapons)
            .HasForeignKey(weapon => weapon.WeaponTypeId);

        modelBuilder.Entity<Attempt>()
            .HasOne(attempt => attempt.User)
            .WithMany()
            .HasForeignKey(attempt => attempt.UserId);

        modelBuilder.Entity<Attempt>()
            .HasOne(attempt => attempt.Weapon)
            .WithMany()
            .HasForeignKey(attempt => attempt.WeaponId);
        
        var adminRole = new UserRole { Id = 1, Name = "Admin" };
        var instructorRole = new UserRole { Id = 2, Name = "Instructor" };
        var clientRole = new UserRole { Id = 3, Name = "Client" };
        
        var admin = new User { Id = 1, Name = "Admin", Password = "Admin", UserRoleId = 1 };
        var instructor = new User { Id = 2, Name = "Instructor", Password = "Instructor", UserRoleId = 2 };
        
        var pistol = new WeaponType { Id = 1, Name = "Pistol" };
        var rifle = new WeaponType { Id = 2, Name = "Rifle" };
        var shotgun = new WeaponType { Id = 3, Name = "Shotgun" };
        
        modelBuilder.Entity<UserRole>().HasData(adminRole, instructorRole, clientRole);
        modelBuilder.Entity<User>().HasData(admin, instructor);
        modelBuilder.Entity<WeaponType>().HasData(pistol, rifle, shotgun);
    }
}