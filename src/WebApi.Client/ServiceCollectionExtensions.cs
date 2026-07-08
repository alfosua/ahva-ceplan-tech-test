using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Ahva.Ceplan.WebApi.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCeplanApiClient(this IServiceCollection services, Action<HttpClient> configureClient)
    {
        services.AddRefitClient<ICeplanAuthApi>().ConfigureHttpClient(configureClient);
        services.AddRefitClient<ICeplanUsersApi>().ConfigureHttpClient(configureClient);

        return services;
    }
}
