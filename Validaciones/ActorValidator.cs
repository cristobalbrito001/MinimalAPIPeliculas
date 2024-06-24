using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class ActorValidator:AbstractValidator<CrearActorDTO>
    {
        public ActorValidator() 
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("el Campo {PropertyName} no debe ser vacio");
            DateTime fechaMinima = new DateTime(1900, 1, 1);
            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima).WithMessage("el Campo {PropertyName} no debe ser superior a "+ fechaMinima.ToString("yyyy-MM-dd"));
        }
    }
}
