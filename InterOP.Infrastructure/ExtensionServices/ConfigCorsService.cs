namespace InterOP.Infrastructure.ExtensionServices
{
    using Microsoft.Extensions.DependencyInjection;
    public static class ConfigCorsService
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
                    //.AllowCredentials()
                    //.Build());
            });
        }
    }
}
