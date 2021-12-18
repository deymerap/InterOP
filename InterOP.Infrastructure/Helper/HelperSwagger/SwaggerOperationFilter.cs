namespace InterOP.Infrastructure.Helper
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Linq;
    public class SwaggerOperationFilter : IOperationFilter
    {
        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    var apiDescription = context.ApiDescription;

        //    operation.Deprecated |= apiDescription.IsDeprecated();

        //    if (operation.Parameters == null)
        //    {
        //        return;
        //    }
        //    var version = apiDescription.GroupName.Replace("v", "");
        //    foreach (var parameter in operation.Parameters)
        //    {
        //        var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
        //        if (description.Name == "version")
        //        {
        //            parameter.Schema.Default = new OpenApiString(version);
        //            parameter.Schema.ReadOnly = true;
        //        }
        //        parameter.Required |= description.IsRequired;
        //    }
        //}


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }

        }
    }
}
