using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem.Model.Entities;

namespace ShootersTrackingSystem.Database;

public class ShootersDbContext : DbContext
{
    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<WeaponType> WeaponTypes { get; set; }

    public ShootersDbContext(DbContextOptions<ShootersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureEntities(modelBuilder);
        InitializeEntities(modelBuilder);
    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasOne(user => user.UserRole).WithMany(userRole => userRole.Users).HasForeignKey(user => user.UserRoleId);
        modelBuilder.Entity<Weapon>().HasOne(weapon => weapon.WeaponType).WithMany(weaponType => weaponType.Weapons).HasForeignKey(weapon => weapon.WeaponTypeId);
        modelBuilder.Entity<Attempt>().HasOne(attempt => attempt.User).WithMany().HasForeignKey(attempt => attempt.UserId);
        modelBuilder.Entity<Attempt>().HasOne(attempt => attempt.Weapon).WithMany().HasForeignKey(attempt => attempt.WeaponId);
    }

    private void InitializeEntities(ModelBuilder modelBuilder)
    {
        const string adminHashedPassword = "mGyd58jTJYJSftGdmF8p/w==$CSSN/KvfydtzYHbK47N3tufiZ72ka3UKD9lbNTwNe7c=";
        const string instructorHashedPassword = "6J8GMK/woJ7lepQ57xBAOg==$T4+RE2fJQOWMTwFft7fz+bLL4+STjR07+XWrJcN/uWk=";
        const string clientHashedPassword = "xEK49XWDwgAjSF1bh3pbLA==$q8I1YPTEl/YBoFCVR3L8JS8ubC2bXluXcU5fRKAv4gQ=";

        var adminRole = new UserRole { Id = 1, Name = "Admin" };
        var instructorRole = new UserRole { Id = 2, Name = "Instructor" };
        var clientRole = new UserRole { Id = 3, Name = "Client" };

        var admin = new User { Id = 1, Name = "Admin", Password = adminHashedPassword, UserRoleId = 1 };
        var instructor = new User { Id = 2, Name = "Instructor", Password = instructorHashedPassword, UserRoleId = 2 };
        
        var client1 = new User { Id = 3, Name = "Client1", Password = clientHashedPassword, UserRoleId = 3 };
        var client2 = new User { Id = 4, Name = "Client2", Password = clientHashedPassword, UserRoleId = 3 };
        var client3 = new User { Id = 5, Name = "Client3", Password = clientHashedPassword, UserRoleId = 3 };

        var pistol = new WeaponType { Id = 1, Name = "Pistol" };
        var rifle = new WeaponType { Id = 2, Name = "Rifle" };
        var shotgun = new WeaponType { Id = 3, Name = "Shotgun" };

        var pm = new Weapon { Id = 1, Name = "ПМ", WeaponTypeId = 1 };
        var ak = new Weapon { Id = 2, Name = "АК-74", WeaponTypeId = 2 };
        var saiga = new Weapon { Id = 3, Name = "Сайга-12", WeaponTypeId = 3 };

        modelBuilder.Entity<UserRole>().HasData(adminRole, instructorRole, clientRole);
        modelBuilder.Entity<User>().HasData(admin, instructor, client1, client2, client3);
        modelBuilder.Entity<WeaponType>().HasData(pistol, rifle, shotgun);
        modelBuilder.Entity<Weapon>().HasData(pm, ak, saiga);

        InitializeAttempts(modelBuilder);
    }

    private void InitializeAttempts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 1, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 2, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 3, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 4, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 50 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 5, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 6, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 7, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 8, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 9, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 10, UserId = 3, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 60 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 11, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 12, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 13, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 14, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 15, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 16, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 17, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 18, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 19, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 20, UserId = 4, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 70 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 21, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 22, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 23, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 24, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 25, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 26, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 27, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 28, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 29, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 30, UserId = 5, WeaponId = 1, DateTime = DateTime.UtcNow, Score = 80 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 31, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 32, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 30 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 33, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 30 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 34, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 35, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 36, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 37, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 30 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 38, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 39, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 40 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 40, UserId = 3, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 40 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 41, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 50 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 42, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 50 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 43, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 50 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 44, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 45, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 46, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 47, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 48, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 49, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 50, UserId = 4, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 51, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 52, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 53, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 54, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 55, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 56, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 30 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 57, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 40 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 58, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 50 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 59, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 60, UserId = 5, WeaponId = 2, DateTime = DateTime.UtcNow, Score = 70 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 61, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 62, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 40 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 63, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 64, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 65, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 66, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 67, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 68, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 69, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 70, UserId = 3, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 30 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 71, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 30 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 72, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 73, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 74, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 75, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 100 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 76, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 77, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 20 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 78, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 60 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 79, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 80, UserId = 4, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 81, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 82, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 83, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 90 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 84, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 85, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 86, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 80 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 87, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 88, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 89, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 70 });
        modelBuilder.Entity<Attempt>().HasData(new Attempt { Id = 90, UserId = 5, WeaponId = 3, DateTime = DateTime.UtcNow, Score = 60 });
    }
}