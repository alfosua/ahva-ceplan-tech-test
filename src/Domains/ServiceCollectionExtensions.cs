using Ahva.Ceplan.Domains.Auth;
using Ahva.Ceplan.Domains.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Ahva.Ceplan.Domains;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCeplanDomainServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<UserCrudService>();
        services.AddScoped<UserListingService>();

        return services;
    }
}
