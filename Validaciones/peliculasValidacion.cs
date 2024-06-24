using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class peliculasValidacion:AbstractValidator<CrearPeliculaDTO>
    {
        public peliculasValidacion()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(Utilidades.NoName)
                .MaximumLength(30).WithMessage(Utilidades.lentgh)
                .MinimumLength(1).WithMessage(Utilidades.minleng);
        }
    }
}
