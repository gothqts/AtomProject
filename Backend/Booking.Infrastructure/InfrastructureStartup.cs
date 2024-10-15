using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContextFactory<BookingDbContext>();
        return services;
    }
}