using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Booking.Infrastructure;

public class BookingDbContext : DbContext
{
    private readonly string _connectionString;
    
    public BookingDbContext(IConfiguration configuration)
    {
        var readedConnString = configuration.GetConnectionString("DefaultConnection");
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
}