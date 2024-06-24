using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class EditarClaimsValidador:AbstractValidator<EditarClaimDTO>
    {
        public EditarClaimsValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.NoName).EmailAddress().WithMessage(Utilidades.EmailMessage);
        }
    }
}
