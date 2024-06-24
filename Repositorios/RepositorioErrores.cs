using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioErrores : IRepositorioErrores
    {
        private readonly ApplicationDbContext _context;
        public RepositorioErrores(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task Crear(Error error)
        {
            _context.Errores.Add(error);
            await _context.SaveChangesAsync();
        }
    }
}
