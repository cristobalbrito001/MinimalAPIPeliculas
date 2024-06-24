using FluentValidation;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Repositorios;

namespace MinimalAPIPeliculas.Validaciones
{
    public class GeneroValidador:AbstractValidator<CrearGeneroDTO>
    {
        public GeneroValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            var valorRuta = httpContextAccessor.HttpContext?.Request.RouteValues["id"];

            int id = 0;
            if(valorRuta is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.Nombre).NotEmpty().WithMessage(Utilidades.NoName)
                                  .MaximumLength(30).WithMessage(Utilidades.lentgh)
                                  .MinimumLength(4).WithMessage(Utilidades.minleng)
                                  .Must(Utilidades.PrimeraLetraEnMayusculas).WithMessage(Utilidades.mayusMessage)
                                  .MustAsync(async (Nombre,_) =>
                                  {
                                      bool exist = await repositorioGeneros.Existe(id,Nombre);
                                      return !exist;
                                  }).WithMessage( g => $"El {g.Nombre} ya existe");

        }
       
    }
}
