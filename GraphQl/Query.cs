using HotChocolate.Authorization;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.GraphQl
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Genero> ObtenerGeneros([Service] ApplicationDbContext context)
        {
            return context.Generos;
        }
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Pelicula> GetPeliculas([Service] ApplicationDbContext context)
        {
            return context.Peliculas;
        }
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Actor> GetActores([Service] ApplicationDbContext context)
        {
            return context.Actores;
        }
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ActorPelicula> GetPeliculaActores([Service] ApplicationDbContext context)
        {
            return context.ActoresPeliculas;
        }
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Comentario> comentarios([Service] ApplicationDbContext context)
        {
            return context.Comentarios;
        }
    }
}
