using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Nox.Core.Interfaces;

namespace Nox.Jobs;

public static class ServiceExtension
{
    public static IServiceCollection AddJobSchedulerFeature(this IServiceCollection services)
    {
        services.AddHangfire((serviceProvider, configuration) =>
        {
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();
            var model = serviceProvider.GetRequiredService<IDynamicModel>();
            var dbProvider = model.GetDatabaseProvider();
            dbProvider.ConfigureJobScheduler(configuration);
            model.SetupRecurringLoaderTasks();
        });
        services.AddHangfireServer();
        return services;
    }
}