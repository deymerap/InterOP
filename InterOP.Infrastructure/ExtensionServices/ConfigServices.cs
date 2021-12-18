using Amazon.S3;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.Interfaces;
using InterOP.Core.ServiceApplications;
using InterOP.Core.Services;
using InterOP.Infrastructure.Repositories;
using InterOP.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InterOP.Infrastructure.ExtensionServices
{
    public static class ConfigServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //Singleton
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IS3Services, S3Service>();
            services.AddRedis(Configuration);

            //Transient

            //Scoped
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IDocumentReceptionService, DocumentReceptionService>();
            services.AddScoped<IDocumentReceptionServiceTask, DocumentReceptionServiceTask>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            //Others Services
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();


            return services;
        }
    }
}
