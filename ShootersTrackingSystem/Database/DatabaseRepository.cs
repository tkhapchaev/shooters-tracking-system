using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Model.Entities;
using ShootersTrackingSystem.Model.Services;

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

        const string adminHashedPassword = "mGyd58jTJYJSftGdmF8p/w==$CSSN/KvfydtzYHbK47N3tufiZ72ka3UKD9lbNTwNe7c=";
        const string instructorHashedPassword = "6J8GMK/woJ7lepQ57xBAOg==$T4+RE2fJQOWMTwFft7fz+bLL4+STjR07+XWrJcN/uWk=";
        const string clientHashedPassword = "HHGY+CdYmsmtAmKLNexZDA==$CmHFJJtLGDqX4ImxPW/bQ8zOB6N3aCwPPqlE4kTb3J4=";
        
        var adminRole = new UserRole { Id = 1, Name = "Admin" };
        var instructorRole = new UserRole { Id = 2, Name = "Instructor" };
        var clientRole = new UserRole { Id = 3, Name = "Client" };
        
        var admin = new User { Id = 1, Name = "Admin", Password = adminHashedPassword, UserRoleId = 1 };
        var instructor = new User { Id = 2, Name = "Instructor", Password = instructorHashedPassword, UserRoleId = 2 };
        var client = new User { Id = 3, Name = "Client", Password = clientHashedPassword, UserRoleId = 3 };
        
        var pistol = new WeaponType { Id = 1, Name = "Pistol" };
        var rifle = new WeaponType { Id = 2, Name = "Rifle" };
        var shotgun = new WeaponType { Id = 3, Name = "Shotgun" };

        var pm = new Weapon { Id = 1, Name = "ПМ", WeaponTypeId = 1 };
        var ak = new Weapon { Id = 2, Name = "АК", WeaponTypeId = 2 };
        var saiga = new Weapon { Id = 3, Name = "Сайга", WeaponTypeId = 3 };
        
        modelBuilder.Entity<UserRole>().HasData(adminRole, instructorRole, clientRole);
        modelBuilder.Entity<User>().HasData(admin, instructor, client);
        modelBuilder.Entity<WeaponType>().HasData(pistol, rifle, shotgun);
        modelBuilder.Entity<Weapon>().HasData(pm, ak, saiga);
    }
}