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
        public static Tbuilder AgregarParametrosFiltroPaginacionOpenApi<Tbuilder>(this Tbuilder tbuilder) where Tbuilder : IEndpointConventionBuilder
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
                    Name = "cantidadRegistrosPorPagina",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "Interger",
                        Default = new OpenApiInteger(10)
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "Titulo",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "GeneroId",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "Interger",
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "EnCines",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "ProximosEstrenos",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "CampoOrdenar",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new List<IOpenApiAny>
                        {
                            new OpenApiString("titulo"),
                            new OpenApiString("fechaLanzamiento"),
                            
                        }
                    }
                });
                options.Parameters.Add(new OpenApiParameter
                {
                    Name = "OrdenAscendente",
                    In = ParameterLocation.Query,
                    Schema = new OpenApiSchema
                    {
                        Type = "boolean",
                        Default = new OpenApiBoolean(true)
                    }
                });
                return options;
            });
        }
    }
}
