using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Filtros;
using MinimalAPIPeliculas.Servicios;
using MinimalAPIPeliculas.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class UsuariosEndPoint
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/registrar", Registrar).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDTO>>();
            groupBuilder.MapPost("/login", Login).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDTO>>();
            groupBuilder.MapPost("/hacerAdmin", HacerAdmin).AddEndpointFilter<FiltroValidaciones<EditarClaimDTO>>().RequireAuthorization("admin");
            groupBuilder.MapPost("/removerAdmin", RemoverAdmin).AddEndpointFilter<FiltroValidaciones<EditarClaimDTO>>().RequireAuthorization("admin");
            groupBuilder.MapGet("/renovar", RenovarToken).RequireAuthorization();

            return groupBuilder;
        }
        static async Task<Results<Ok<RespuestaAuntenticacionDTO>, BadRequest<IEnumerable<IdentityError>>>> Registrar(CredencialesUsuarioDTO credencialesUsuario, 
                                                                                            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email,
            };
            var result = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (result.Succeeded)
            {
                var credencialesRespuesta = await ContruirToken(credencialesUsuario, configuration,userManager);
                return TypedResults.Ok(credencialesRespuesta);
            }
            else
            {
               return TypedResults.BadRequest(result.Errors);
            }
        }
        static async Task<Results<Ok<RespuestaAuntenticacionDTO>,BadRequest<string>>> Login(CredencialesUsuarioDTO credencialesUsuarioDTO, 
                                                        [FromServices] SignInManager<IdentityUser> signInManager, [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration
                                                        )
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            if (usuario is null)
            {
                return TypedResults.BadRequest("Login Incorrecto");
            }
            var result = await signInManager.CheckPasswordSignInAsync(usuario,credencialesUsuarioDTO.Password, lockoutOnFailure:false);
            if (result.Succeeded)
            {
                var responseAutentication = await  ContruirToken(credencialesUsuarioDTO, configuration, userManager);
                return TypedResults.Ok(responseAutentication);
            }
            else
            {
                return TypedResults.BadRequest("login Incorrecto");
            }
        }
        private async static Task<RespuestaAuntenticacionDTO> ContruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credencialesUsuarioDTO.Email)

            };
            var usuario = await userManager.FindByNameAsync(credencialesUsuarioDTO.Email);
            var claimsDb = await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDb);
            var Llave = Llaves.ObtenerLlave(configuration);
            var cred = new SigningCredentials(Llave.First(), SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);
            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims:claims, expires:expiracion, signingCredentials: cred);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);
            return new RespuestaAuntenticacionDTO
            {
                Token = token,
                Experiracion = expiracion
            };
        }

        static async Task<Results<NoContent,NotFound>> HacerAdmin(EditarClaimDTO editarClaimDTO, [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);
            if(usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.AddClaimAsync(usuario, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> RemoverAdmin(EditarClaimDTO editarClaimDTO, [FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);
            if (usuario is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.RemoveClaimAsync(usuario, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        public async static Task<Results<Ok<RespuestaAuntenticacionDTO>,NotFound>> RenovarToken(IServicioUsuario servicioUsuario, 
            IConfiguration configuration,[FromServices] UserManager<IdentityUser> userManager)
        {
            var usuario = await servicioUsuario.ObtenerUsuario();
            if(usuario is null)
            {
                return TypedResults.NotFound();
            }
            CredencialesUsuarioDTO credencialesUsuarioDTO = new CredencialesUsuarioDTO { Email = usuario.Email!};
            RespuestaAuntenticacionDTO resp = await ContruirToken(credencialesUsuarioDTO, configuration,userManager);
            return TypedResults.Ok(resp);
        }
    }
}
