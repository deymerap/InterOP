namespace InterOP.Infrastructure.ExtensionServices
{
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;

    public static class ConfigSwaggerApp
    {
        public static void UseConfigSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(confing =>
            {
                ////confing.SwaggerEndpoint("/swagger/v1/swagger.json", "SIESA FE. - Interoperabilida API Test");
                //confing.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
                //confing.RoutePrefix = string.Empty;

                foreach (var apiVersion in provider.ApiVersionDescriptions.OrderBy(version => version.ToString()))
                {
                    confing.SwaggerEndpoint($"/swagger/{apiVersion.GroupName}/swagger.json",
                            $"{apiVersion.GroupName}"
                    );
                }
                confing.RoutePrefix = "docs-sw";

            });
        }


        ////public static void UseConfigReDoc(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        ////{
        ////    app.UseReDoc(c =>
        ////    {
        ////        c.RoutePrefix = "docs-redoc";
        ////        foreach (var apiVersion in provider.ApiVersionDescriptions.OrderBy(version => version.ToString()))
        ////        {
        ////            c.SpecUrl($"/swagger/{apiVersion.GroupName}/swagger.json");
        ////        }

        ////        c.ConfigObject = new ConfigObject
        ////        {
        ////            HideDownloadButton = true,
        ////            HideLoading = true                    
        ////        };
        ////    });
        ////}
    }
}
