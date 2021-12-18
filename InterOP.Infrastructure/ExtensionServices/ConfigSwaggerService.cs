namespace InterOP.Infrastructure.ExtensionServices
{
    using InterOP.Core.Enumerations;
    using InterOP.Infrastructure.Helper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.IO;
    using System.Reflection;

    public static class ConfigSwaggerService
    {
        public static void ConfigureSwagger(this IServiceCollection services, ActiveAuthSwaggerUI activeAuthSwaggerUI = ActiveAuthSwaggerUI.None)
        {
            services.AddSwaggerGen(options =>
            {
                var vObProvider = services.BuildServiceProvider()
                                        .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var apiVersion in vObProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(apiVersion.GroupName, CreateInfoForApiVersion(apiVersion));
                }

                options.OperationFilter<SwaggerOperationFilter>();
                options.DocumentFilter<SwaggerDocumentFilter>();

                // Set the comments path for the Swagger JSON and UI.
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml"));

                if (activeAuthSwaggerUI == ActiveAuthSwaggerUI.Active)
                {
                    // add JWT Authentication
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "JWT Authentication",
                        Description = "Ingresar JWT Bearer token **_only_**",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // must be lower case
                        BearerFormat = "JWT",
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };

                    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });

                    //// add Basic Authentication
                    //var basicSecurityScheme = new OpenApiSecurityScheme
                    //{
                    //    Type = SecuritySchemeType.Http,
                    //    Scheme = "basic",
                    //    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                    //};

                    //options.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    //{
                    //    {basicSecurityScheme, new string[] { }}
                    //});
                }

            });
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription pvStrDescription)
        {
            var vApIinfo = new OpenApiInfo()
            {
                Version = pvStrDescription.ApiVersion.ToString(),
                Title = "SIESA FE - InterOP API",
                Description = "Documentacion API.",
                Contact = new OpenApiContact { Name = "Deymer A. Perea", Email = "deymer.perea@siesa.com" }
            };

            if (pvStrDescription.IsDeprecated)
            {
                vApIinfo.Description += "- Esta API es una version deprecada.";
            }

            return vApIinfo;
        }
    }
}
