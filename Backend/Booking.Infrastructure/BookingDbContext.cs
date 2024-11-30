using Booking.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Booking.Infrastructure;

public class BookingDbContext : DbContext
{
    private readonly string _connectionString;
    
    public DbSet<DynamicFieldType> DynamicFieldTypes { get; init; }
    public DbSet<EntryFieldValue> EntryFieldValues { get; init; }
    public DbSet<EventSignupEntry> EventSignupEntries { get; init; }
    public DbSet<EventSignupForm> EventSignupForms { get; init; }
    public DbSet<EventSignupWindow> EventSignupWindows { get; init; }
    public DbSet<FormDynamicField> FormDynamicFields { get; init; }
    public DbSet<OrganizerContacts> OrganizerContacts { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<UserEvent> UsersEvents { get; init; }
    public DbSet<UserRole> UsersRoles { get; init; }
    public DbSet<RefreshToken> RefreshTokens { get; init; }
    public DbSet<RevokedAccessToken> RevokedAccessTokens { get; init; }
    
    public BookingDbContext(IConfiguration configuration)
    {
        var readedConnString = configuration.GetConnectionString("PostgresDb");
        if (readedConnString is null)
        {
            throw new Exception("Connection string \"PostgresDb\" wasn't found in appsettings.json");
        }
        _connectionString = readedConnString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(_connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Добавление начальных данных
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                Id = Guid.NewGuid(),
                Title = "User",
                CanEditOthersEvents = false,
                IsAdmin = false
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                Title = "Admin",
                CanEditOthersEvents = true,
                IsAdmin = true
            },
            new UserRole
            {
                Id = Guid.NewGuid(),
                Title = "Moderator",
                CanEditOthersEvents = true,
                IsAdmin = false
            }
        );
        
        modelBuilder.Entity<DynamicFieldType>().HasData(
            new DynamicFieldType
            {
                Id = Guid.NewGuid(),
                Title = "string"
            },
            new DynamicFieldType
            {
                Id = Guid.NewGuid(),
                Title = "number"
            }
        );
    }
}