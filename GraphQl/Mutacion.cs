using AutoMapper;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.GraphQl
{
    public class Mutacion
    {
        [Serial]
        public async Task<GeneroDTO> CrearGenero([Service] IRepositorioGeneros repositorio, [Service] IMapper mapper, CrearGeneroDTO crearGeneroDTO)
        {
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            await repositorio.Crear(genero);
            return mapper.Map<GeneroDTO>(genero);
        }
        [Serial]
        public async Task<GeneroDTO> UpdateGenero([Service] IRepositorioGeneros repositorio, [Service] IMapper mapper, ActualizarGeneroDto crearGeneroDTO)
        {
            var existe = await repositorio.Existe(crearGeneroDTO.Id);
            if (!existe)
            {
                throw new Exception("El género no existe en la base de datos");
            }
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            await repositorio.Actualizar(genero);
            return mapper.Map<GeneroDTO>(genero);
        }
        [Serial]
        public async Task<bool> DeleteGenero([Service] IRepositorioGeneros repositorio, [Service] IMapper mapper, int Id)
        {
            var existe = await repositorio.Existe(Id);
            if (!existe)
            {
                throw new Exception("El género no existe en la base de datos");
            }
           
            await repositorio.Borrar(Id);
            return existe;
        }
    }
}
