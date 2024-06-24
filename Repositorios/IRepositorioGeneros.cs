using MinimalAPIPeliculas.Entidades;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MinimalAPIPeliculas.Repositorios
{
    public interface IRepositorioGeneros
    {
        Task<List<Genero>> ObtenerTodos();
        Task<Genero?> ObtenerPorId(int id);
        Task<int> Crear(Genero genero);
        Task<bool> Existe(int id);
        Task<List<int>> Existen(List<int> ids);
        Task Actualizar(Genero genero);
        Task Borrar(int id);
        Task<bool> Existe( int Id,string Nombre);
    }
}
