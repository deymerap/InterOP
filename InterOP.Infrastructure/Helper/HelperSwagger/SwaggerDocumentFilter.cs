namespace InterOP.Infrastructure.Helper
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Linq;
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        //public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        //{
        //    var Paths = swaggerDoc.Paths
        //           .ToDictionary(   
        //               path => path.Key.Replace("{version}", swaggerDoc.Info.Version),
        //               path => path.Value
        //           );

        //    OpenApiPaths vRutas = new OpenApiPaths();
        //    foreach (var vItem in Paths)
        //        vRutas.Add(vItem.Key, vItem.Value);
        //    swaggerDoc.Paths = vRutas;
        //    vRutas = null;
        //}

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pahts = swaggerDoc.Paths;
            swaggerDoc.Paths = new OpenApiPaths();
            foreach (var paht in pahts)
            {
                var key = paht.Key.Replace("{version}", swaggerDoc.Info.Version);
                var value = paht.Value;
                swaggerDoc.Paths.Add(key, value);
            }
        }
    }
}

