using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContextFactory<BookingDbContext>();
        return services;
    }

    public static void CheckAndMigrateDatabase(IServiceScope scope)
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}