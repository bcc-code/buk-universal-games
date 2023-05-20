using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Buk.UniversalGames.Api.Authorization
{
    public class TeamCodeHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-ubg-teamcode",
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}
