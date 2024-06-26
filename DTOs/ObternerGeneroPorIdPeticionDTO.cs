using AutoMapper;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.DTOs
{
    public class ObternerGeneroPorIdPeticionDTO
    {
        public IRepositorioGeneros? repositorio { get; set; }
        public int id { get; set; }
        public IMapper? mapper { get; set; }
        public IOutputCacheStore? outputCacheStore { get; set; }
    }
}
