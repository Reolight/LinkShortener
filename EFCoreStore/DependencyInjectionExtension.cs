using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreStore;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddMySqlWithEfCoreStore(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseMySql(connectionString, ServerVersion.Parse("8.1")));
        return services;
    }
}