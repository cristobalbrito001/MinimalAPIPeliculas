using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIPeliculas.Utilidades
{
    public static class HttpContextExtensionsUtilidades
    {
        public static T ExtraerValoresDefecto<T>(this HttpContext context, string nombreDelCampo, T valorDefecto)where T : IParsable<T>
        {
            var valor = context.Request.Query[nombreDelCampo];
            if (valor.IsNullOrEmpty())
            {
                return valorDefecto;
            }
            return T.Parse(valor!, null);
        }
    }
}
