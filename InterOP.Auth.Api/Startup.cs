namespace InterOP.Auth.Api
{
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using InterOP.Core.Enumerations;
    using InterOP.Core.OptionApplications;
    using InterOP.Infrastructure.ExtensionServices;
    using InterOP.Infrastructure.Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;

    public class Startup
    {
        public static readonly ILoggerFactory loggerFactory
           = LoggerFactory.Create(builder =>
           {
               builder
                   .AddFilter((category, level) =>
                       category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information)
                .AddConsole();
           });

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddScoped<IDataAccess, DataAccess>();
            services.ConfigureSqlServerDbContext(Configuration, loggerFactory);
            services.AddServices(Configuration);
            services.ConfigureCors();

            services.AddControllers().ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true; //Quitar la validacion de los modelos  
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            //Mapping Json Configurations of PasswordOptions
            services.Configure<PasswordOptions>(Configuration.GetSection("ServicesSecurity:PasswordOptions"));
            services.Configure<AwsS3Options>(Configuration.GetSection("ServicesSecurity:AwsS3BucketOptions"))
               .AddSingleton(x => x.GetRequiredService<IOptions<AwsS3Options>>().Value);

            //Authentication JWT
            services.ConfigAuthentication(Configuration);
            
            services.AddMvcCore(op =>
            {
                op.Filters.Add<ValidationFilter>();
            })
            //Validations
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            //Api Versioning
            var apiExplorer = services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";//The format of the version added to the route URL                  
                options.SubstituteApiVersionInUrl = true;//Tells swagger to replace the version in the controller route  
            });
            apiExplorer.ConfigureApiVersioning();

            //Api Documentations
            services.ConfigureSwagger(ActiveAuthSwaggerUI.None);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            //Add Swagger
            app.UseConfigSwagger(provider);
            //app.UseConfigReDoc(provider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Cors
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles(); //permite el uso de archivos estáticos para la solicitud.
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


