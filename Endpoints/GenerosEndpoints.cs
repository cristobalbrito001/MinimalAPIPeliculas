using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Filtros;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"))
               ;
            group.MapGet("/{id:int}", ObtenerGeneroPorId);
            group.MapPost("/", CrearGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>().RequireAuthorization("admin");
            group.MapPut("/{id:int}", ActualizarGenero).AddEndpointFilter<FiltroValidaciones<CrearGeneroDTO>>().RequireAuthorization("admin").WithOpenApi(options =>
            {
                options.Summary = "Actualiza un género";
                options.Description = "Actualiza un género existente";
                options.Parameters[0].Description = "El id del género a actualizar";
                options.RequestBody.Description = "El género a actualizar";
                return options;
            });
            group.MapDelete("/{id:int}", BorrarGenero).RequireAuthorization("admin");
            return group;
        }

        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repositorio, IMapper mapper,ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger(typeof(GenerosEndpoints));
           
            var generos = await repositorio.ObtenerTodos();
            logger.LogInformation("Obteniendo los géneros");
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generosDTO);
        }

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId([AsParameters] ObternerGeneroPorIdPeticionDTO obternerGeneroPorIdPeticionDTO )
        {
            var genero = await obternerGeneroPorIdPeticionDTO.repositorio!.ObtenerPorId(obternerGeneroPorIdPeticionDTO.id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            var generoDTO = obternerGeneroPorIdPeticionDTO.mapper!.Map<GeneroDTO>(genero);

            return TypedResults.Ok(generoDTO);
        }

        static async Task<Results<Created<GeneroDTO>, ValidationProblem>> CrearGenero(CrearGeneroDTO crearGeneroDTO, [AsParameters] CrearGeneroRequest peticionDTO 
           )
        {
           
            
            var genero = peticionDTO.mapper!.Map<Genero>(crearGeneroDTO);
            var id = await peticionDTO.repositorio!.Crear(genero);
            await peticionDTO.outputCacheStore!.EvictByTagAsync("generos-get", default);
            var generoDTO = peticionDTO.mapper!.Map<GeneroDTO>(genero);
            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        static async Task<Results<NoContent, NotFound, ValidationProblem,BadRequest>> ActualizarGenero(int id, 
            CrearGeneroDTO crearGeneroDTO,
            IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            try
            {
                
                var existe = await repositorio.Existe(id);

                if (!existe)
                {
                    return TypedResults.NotFound();
                }

                var genero = mapper.Map<Genero>(crearGeneroDTO);
                genero.Id = id;

                await repositorio.Actualizar(genero);
                await outputCacheStore.EvictByTagAsync("generos-get", default);
                return TypedResults.NoContent();
            } catch (Exception ex)
            {
                return TypedResults.BadRequest();
            }
        }

        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }
}
