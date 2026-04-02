using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sprintFlow.Infrastructure.Persistence;

namespace sprintFlow.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SprintFlowDatabase");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

    }
}
