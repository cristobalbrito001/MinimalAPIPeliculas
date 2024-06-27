using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.Utilidades;

namespace MinimalAPIPeliculas.DTOs
{
    public class PaginacionDTO
    {
        private const int paginaValorInicial = 1;
        private const int recordsPorPaginaValorInicial = 10;
        public int Pagina { get; set; } = paginaValorInicial;
        public int recordsPorPagina = recordsPorPaginaValorInicial;
        private readonly int cantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
        public static ValueTask<PaginacionDTO> BindAsync(HttpContext httpContext)
        {
            var pagina = httpContext.ExtraerValoresDefecto(nameof(Pagina), paginaValorInicial);
            var RecordsPorPagina = httpContext.ExtraerValoresDefecto(nameof(recordsPorPagina), recordsPorPaginaValorInicial);

            
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = RecordsPorPagina };
            return ValueTask.FromResult(paginacion);
        }
    }
}
