using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Migrations;
using MinimalAPIPeliculas.Utilidades;
using System.Linq.Dynamic.Core;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioPeliculas : IRepositorioPeliculas
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly HttpContext httpContext;
        private readonly ILogger<RepositorioPeliculas> logger;

        public RepositorioPeliculas(ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<RepositorioPeliculas> logger)
        {
            this.context = context;
            this.mapper = mapper;
            httpContext = httpContextAccessor.HttpContext!;
            this.logger = logger;
        }

        public async Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Peliculas.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(p => p.Titulo).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Pelicula?> ObtenerPorId(int id)
        {
            return await context.Peliculas.Include(p => p.Comentarios)
                .Include(p => p.GeneroPeliculas)
                    .ThenInclude(g => g.Genero)
                .Include(p => p.ActorPelicula.OrderBy(p => p.Orden))
                    .ThenInclude(p => p.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Crear(Pelicula pelicula)
        {
            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        public async Task Actualizar(Pelicula pelicula)
        {
            context.Update(pelicula);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Peliculas.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Peliculas.AnyAsync(p => p.Id == id);
        }

        public async Task AsignarGeneros(int id, List<int> generosIDs)
        {
            var pelicula = await context.Peliculas.Include(p => p.GeneroPeliculas).FirstOrDefaultAsync(p => p.Id == id);
            if(pelicula is null)
            {
                throw new ArgumentException($"No existe pelicula con el id {id}");
            }
            var generosPeliculas = generosIDs.Select(generoId => new GeneroPelicula() { GeneroId = generoId });

            pelicula.GeneroPeliculas = mapper.Map(generosPeliculas, pelicula.GeneroPeliculas);
            await context.SaveChangesAsync();
        }
       public async Task AsignarActores(int id, List<ActorPelicula> actores)
        {

            for(int i = 1; i <= actores.Count; i++)
            {
                actores[i-1].Orden = i;
            }
            var pelicula = await context.Peliculas.Include(p => p.ActorPelicula).FirstOrDefaultAsync(p => p.Id == id);
            if(pelicula is null)
            {
                throw new ArgumentException($"no existe la pelicula con id {id}");
            }
            pelicula.ActorPelicula = mapper.Map(actores, pelicula.ActorPelicula);
            await context.SaveChangesAsync();
        }

        public  async Task<List<Pelicula>> Filtrar(PeliculaFilterDTO peliculaFilterDTO)
        {
           var peliculasQuerible =  context.Peliculas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(peliculaFilterDTO.Titulo))
            {
                peliculasQuerible = peliculasQuerible.Where(p => p.Titulo.Contains(peliculaFilterDTO.Titulo));
            }
            if (peliculaFilterDTO.EnCines)
            {
                peliculasQuerible = peliculasQuerible.Where(p => p.EnCines);
            }
            if (peliculaFilterDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQuerible = peliculasQuerible.Where(p => p.FechaLanzamiento > hoy);
            }
            if (peliculaFilterDTO.GeneroId != 0)
            {
                peliculasQuerible = peliculasQuerible.Where(p => p.GeneroPeliculas.Select(gp => gp.GeneroId).Contains(peliculaFilterDTO.GeneroId));
            }
            if (!string.IsNullOrWhiteSpace(peliculaFilterDTO.CampoOrdenar))
            {
               var tipoOrder = peliculaFilterDTO.OrdenAscendente ? "ascending" : "desending";
                try
                {
                    peliculasQuerible = peliculasQuerible.OrderBy($"{peliculaFilterDTO.CampoOrdenar} {tipoOrder}");
                }
                catch
                {
                    logger.LogError("No se pudo ordenar por {0}", peliculaFilterDTO.CampoOrdenar);
                }
            }
            await httpContext.InsertarParametrosPaginacionEnCabecera(peliculasQuerible);
            var peliculas = await peliculasQuerible.Paginar(peliculaFilterDTO.paginacionDTO).ToListAsync();
            return peliculas;
        }
    }
}
