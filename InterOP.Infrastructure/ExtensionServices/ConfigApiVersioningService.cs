namespace InterOP.Infrastructure.ExtensionServices
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigApiVersioningService
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true; //informar versionen Header
                options.DefaultApiVersion = new ApiVersion(1, 0); // version por defecto
                options.AssumeDefaultVersionWhenUnspecified = true; // tomar la version  por defecto  si no se especifica

                options.ApiVersionReader = ApiVersionReader.Combine(//new QueryStringApiVersionReader(),
                                                                    //new HeaderApiVersionReader("api-version"),
                                                                     new MediaTypeApiVersionReader("v"));
            });
        }

        public static void ConfigureApiVersioningExplorer(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options =>
            {
                //The format of the version added to the route URL  
                options.GroupNameFormat = "'v'VVV";
                //Tells swagger to replace the version in the controller route  
                options.SubstituteApiVersionInUrl = true;
            });

        }
    }
}
