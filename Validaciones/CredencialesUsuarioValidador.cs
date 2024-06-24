using FluentValidation;
using MinimalAPIPeliculas.DTOs;

namespace MinimalAPIPeliculas.Validaciones
{
    public class CredencialesUsuarioValidador: AbstractValidator<CredencialesUsuarioDTO>
    {
        public CredencialesUsuarioValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Utilidades.NoName).MaximumLength(256)
                .WithMessage(Utilidades.lentgh).MinimumLength(6).WithMessage(Utilidades.minleng).EmailAddress().WithMessage(Utilidades.EmailMessage);


            RuleFor(x => x.Password).NotEmpty().WithMessage(Utilidades.NoName).MaximumLength(125).WithMessage(Utilidades.lentgh).MinimumLength(6).WithMessage(Utilidades.minleng);
        }
    }
}
