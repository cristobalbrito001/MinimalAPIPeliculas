using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repositorios
{
    public interface IRepositorioErrores
    {
        Task Crear(MinimalAPIPeliculas.Entidades.Error error);
    }
}
