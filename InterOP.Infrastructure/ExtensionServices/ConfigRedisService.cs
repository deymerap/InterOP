using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace InterOP.Infrastructure.ExtensionServices
{
    public static class ConfigRedisService
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration Configuration)
        {
            var vObRedisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddSingleton(vObRedisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            //services.AddSingleton<IRedisDatabase, RedisDatabase>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();

            return services;
        }
    }
}
