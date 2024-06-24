
using AutoMapper;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.Filtros
{
    public class FiltroPrueba : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {

            var param1 =  context.Arguments.OfType<IRepositorioGeneros>().FirstOrDefault();
            var param2 = context.Arguments.OfType<int>().FirstOrDefault(); ;
            var param3 = context.Arguments.OfType<IMapper>().FirstOrDefault(); ;
            var result = await next(context);
            return result;
        }
    }
}
