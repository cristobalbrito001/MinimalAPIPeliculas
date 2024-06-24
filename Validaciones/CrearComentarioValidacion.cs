using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class CrearComentarioValidacion : AbstractValidator<CrearComentarioDTO>
    {
        public CrearComentarioValidacion()
        {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(Utilidades.NoName);
        }
    }
}
