using MinimalAPIPeliculas.Utilidades;

namespace MinimalAPIPeliculas.DTOs
{
    public class PeliculaFilterDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina = 10;
        public PaginacionDTO paginacionDTO
        {
            get
            {
                return new PaginacionDTO() { Pagina = Pagina, recordsPorPagina = cantidadRegistrosPorPagina };
            }
        }
        public string? Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
        public string? CampoOrdenar { get; set; }
        public bool OrdenAscendente { get; set; } = true;
        public static ValueTask<PeliculaFilterDTO> BindAsync(HttpContext httpContext)
        {
            var pagina = httpContext.ExtraerValoresDefecto(nameof(Pagina), 1);
            var recordsPorPagina = httpContext.ExtraerValoresDefecto(nameof(cantidadRegistrosPorPagina), 10);
            var titulo = httpContext.ExtraerValoresDefecto(nameof(Titulo), string.Empty);
            var generoId = httpContext.ExtraerValoresDefecto(nameof(GeneroId), 0);
            var enCines = httpContext.ExtraerValoresDefecto(nameof(EnCines), false);
            var proximosEstrenos = httpContext.ExtraerValoresDefecto(nameof(ProximosEstrenos), false);
            var campoOrdenar = httpContext.ExtraerValoresDefecto(nameof(CampoOrdenar), string.Empty);
            var ordenAscendente = httpContext.ExtraerValoresDefecto(nameof(OrdenAscendente), true);
            var peliculaFilterDTO = new PeliculaFilterDTO
            {
                Pagina = pagina,
                cantidadRegistrosPorPagina = recordsPorPagina,
                Titulo = titulo,
                GeneroId = generoId,
                EnCines = enCines,
                ProximosEstrenos = proximosEstrenos,
                CampoOrdenar = campoOrdenar,
                OrdenAscendente = ordenAscendente
            };
            return ValueTask.FromResult(peliculaFilterDTO);
        }
    }
}
