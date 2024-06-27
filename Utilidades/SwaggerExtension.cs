using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIPeliculas.Utilidades
{
    public static class SwaggerExtension
    {
        public static Tbuilder AgregarParametrosPaginacionOpenApi<Tbuilder>(this Tbuilder tbuilder) where Tbuilder: IEndpointConventionBuilder
        {
            return tbuilder.WithOpenApi(options =>
            {
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Pagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "Interger",
                        Default = new OpenApiInteger(1)
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "recordsPorPagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "Interger",
                        Default = new OpenApiInteger(10)
                    }
                });
                return options;
            });
        }
    }
}
