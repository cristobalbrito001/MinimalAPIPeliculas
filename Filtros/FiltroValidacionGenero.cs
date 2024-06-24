﻿
using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Filtros
{
    public class FiltroValidacionGenero : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validador = context.HttpContext.RequestServices.GetService<IValidator<CrearGeneroDTO>>();
            if (validador == null)
            {
                return await next(context);
            }
            var parametroGenero = context.Arguments.OfType<CrearGeneroDTO>().FirstOrDefault();
            if(parametroGenero is null)
            {
                return TypedResults.Problem("no pudo ser encontrada la entidad a validar");
            }
            var resultValidate = await validador.ValidateAsync(parametroGenero);
            if (!resultValidate.IsValid)
            {
                return TypedResults.ValidationProblem(resultValidate.ToDictionary());
            }
            return await next(context);
        }
    }
}
