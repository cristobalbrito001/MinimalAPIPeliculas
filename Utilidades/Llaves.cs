using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIPeliculas.Utilidades
{
    public static class Llaves
    {
        public const string IssuePropio = "secret-key-app";
        public const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";
        public const string SeccionLlavesEmisor = "Issuer";
        public const string SeccionLlavesValue = "Value";


        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration) => ObtenerLlave(configuration, IssuePropio);

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issue)
        {
            var signingKey = configuration.GetSection(SeccionLlaves).GetChildren().SingleOrDefault(Key => Key[SeccionLlavesEmisor] == issue);


            if(signingKey is not null && signingKey[SeccionLlavesValue] is string valorLlave)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
            }

        }
        public static IEnumerable<SecurityKey> ObtenerTodasLlave(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(SeccionLlaves).GetChildren();

            foreach(var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlavesValue] is string valorLlave)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
                }
            }
        }


    }
}
