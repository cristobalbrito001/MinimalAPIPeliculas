using AutoMapper;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Migrations;

namespace MinimalAPIPeliculas.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearGeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();

            CreateMap<CrearActorDTO, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();

            CreateMap<CrearPeliculaDTO, Pelicula>()
              .ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<Pelicula, PeliculaDTO>().ForMember(p => p.generos, entidad =>
            entidad.MapFrom(p => p.GeneroPeliculas.Select(gp =>
            new GeneroDTO { Id = gp.GeneroId, Nombre = gp.Genero.Nombre })))
                .ForMember(p => p.Actores, entidad => 
                    entidad.MapFrom(p => p.ActorPelicula.Select(ga => new ActorPeliculaDTOs { Id = ga.ActorId, Nombre = ga.Actor.Nombre, Personaje = ga.Personaje })));

              

            CreateMap<CrearComentarioDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();

            CreateMap<AsignarActorPeliculaDTOs, ActorPelicula>().ReverseMap();

            

        }
    }
}
