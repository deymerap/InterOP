using InterOP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InterOP.Infrastructure.ExtensionServices
{
    public static class ConfigSqlServerDbContext
    {
        public static void ConfigureSqlServerDbContext(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            services.AddDbContextPool<InterOPDevContext>(cfg =>
            {
                cfg.UseSqlServer(configuration["ConfigDatabase:ConnectionStrings:LocalBD"])
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(loggerFactory);
            });
        }
    }
}
