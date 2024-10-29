using Booking.Application.Services;
using Booking.Application.Services.AuthService;
using Booking.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Booking.Application;

public static class ApplicationStartup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IAuthService, AuthService>();
        
        services.TryAddScoped<BaseService<DynamicFieldType>>();
        services.TryAddScoped<BaseService<EntryFieldValue>>();
        services.TryAddScoped<BaseService<EventSignupEntry>>();
        services.TryAddScoped<BaseService<EventSignupForm>>();
        services.TryAddScoped<BaseService<EventSignupWindow>>();
        services.TryAddScoped<BaseService<FormDynamicField>>();
        services.TryAddScoped<BaseService<OrganizerContacts>>();
        services.TryAddScoped<BaseService<User>>();
        services.TryAddScoped<BaseService<UserEvent>>();
        services.TryAddScoped<BaseService<UserRole>>();
        services.TryAddScoped<UserInfoService>();
        
        return services;
    }
}