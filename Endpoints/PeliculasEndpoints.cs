using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Filtros;
using MinimalAPIPeliculas.Migrations;
using MinimalAPIPeliculas.Repositorios;
using MinimalAPIPeliculas.Servicios;
using MinimalAPIPeliculas.Utilidades;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class PeliculasEndpoints
    {
        private static readonly string contenedor = "peliculas";

        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", Obtener).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get")).AgregarParametrosPaginacionOpenApi() ;
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("admin");
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearPeliculaDTO>>().RequireAuthorization("admin");
            group.MapDelete("/{id:int}", Borrar).RequireAuthorization("admin");
            group.MapPost("/{id:int}/AsignarGeneros", AsignarGeneros).RequireAuthorization("admin");
            group.MapPost("/{id:int}/AsignarActores", AsignarActores).RequireAuthorization("admin");
            return group;
        }

        static async Task<Ok<List<PeliculaDTO>>> Obtener(IRepositorioPeliculas repositorio,
            PaginacionDTO paginacionDTO, IMapper mapper)
        {
            
            var peliculas = await repositorio.ObtenerTodos(paginacionDTO);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculas);
            return TypedResults.Ok(peliculasDTO);
        }

        static async Task<Results<Ok<PeliculaDTO>, NotFound>> ObtenerPorId(int id, IRepositorioPeliculas repositorio,
            IMapper mapper)
        {
            var pelicula = await repositorio.ObtenerPorId(id);

            if (pelicula is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Ok(peliculaDTO);
        }

        static async Task<Results<Created<PeliculaDTO>, BadRequest<string>>> Crear([FromForm] CrearPeliculaDTO crearPeliculaDTO,
            IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            try { 
            var pelicula = mapper.Map<Pelicula>(crearPeliculaDTO);

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorio.Crear(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Created($"/peliculas/{id}", peliculaDTO);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return TypedResults.BadRequest("ERROR EN EL LLAMADO A LA API");
            }
        }

        static async Task<Results<NoContent, NotFound>> Actualizar(int id, 
            [FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorio,
            IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaParaActualizar = mapper.Map<Pelicula>(crearPeliculaDTO);
            peliculaParaActualizar.Id = id;
            peliculaParaActualizar.Poster = peliculaDB.Poster;

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Editar(peliculaParaActualizar.Poster,
                    contenedor, crearPeliculaDTO.Poster);
                peliculaParaActualizar.Poster = url;
            }

            await repositorio.Actualizar(peliculaParaActualizar);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioPeliculas repositorio,
            IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var peliculaDB = await repositorio.ObtenerPorId(id);

            if (peliculaDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(peliculaDB.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }
        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarGeneros(int id,List<int> generosIds, IRepositorioPeliculas repositorioPeliculas, IRepositorioGeneros repositorioGeneros)
        {
            if ( !await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }

            var  generosExistentes = new List<int>();
            if(generosIds.Count != 0)
            {
                generosExistentes = await repositorioGeneros.Existen(generosIds);
            }
            if(generosExistentes.Count != generosIds.Count)
            {
                var generosNoExistentes = generosIds.Except(generosExistentes);

                return TypedResults.BadRequest($"hay generos que no existen{string.Join(",", generosNoExistentes)}");

            }

            await repositorioPeliculas.AsignarGeneros(id, generosIds);
            return TypedResults.NoContent();

        }

        static async Task<Results<NotFound, NoContent, BadRequest<string>>> AsignarActores(int id, List<AsignarActorPeliculaDTOs> actoresDTOs, IRepositorioPeliculas repositorioPeliculas, IRepositorioActores repositorioActores, IMapper mapper)
        {
            if(!await repositorioPeliculas.Existe(id))
            {
                return TypedResults.NotFound();
            }     
            var actoresExistentes = new List<int>();
            var actoresIds =  actoresDTOs.Select(a => a.ActorID).ToList();
            if (actoresDTOs.Count != 0)
            {
                actoresExistentes = await repositorioActores.Existen(actoresIds);
            }
            if (actoresExistentes.Count != actoresDTOs.Count)
            {
                var actoresNoexistentes = actoresIds.Except(actoresExistentes);
                return TypedResults.BadRequest($"no existen los actores {string.Join(",", actoresNoexistentes)}");
            }
            var actores = mapper.Map<List<ActorPelicula>>(actoresDTOs);
            await repositorioPeliculas.AsignarActores(id, actores);
            return TypedResults.NoContent();
        }

        
    }
}
