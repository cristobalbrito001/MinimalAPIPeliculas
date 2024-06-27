﻿using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repositorios
{
    public interface IRepositorioPeliculas
    {
        Task Actualizar(Pelicula pelicula);
        Task Borrar(int id);
        Task<int> Crear(Pelicula pelicula);
        Task<bool> Existe(int id);
        Task<Pelicula?> ObtenerPorId(int id);
        Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO);
        Task AsignarGeneros(int id, List<int> generosIDs);
        Task AsignarActores(int id, List<ActorPelicula> actores);
        Task<List<Pelicula>> Filtrar(PeliculaFilterDTO peliculaFilterDTO);
    }
}
