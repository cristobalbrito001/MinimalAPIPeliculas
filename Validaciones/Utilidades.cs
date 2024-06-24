namespace MinimalAPIPeliculas.Validaciones
{
    public class Utilidades
    {
        public static string NoName = "el Campo {PropertyName} no debe ser vacio";
        public static string lentgh = "el {PropertyName} debe ser menos a {MaxLength} caracteres";
        public static string minleng = "el {PropertyName} debe ser mayor a {MinLength} caracteres ";
        public static string mayusMessage = "el {PropertyName} debe empezar con mayusculas";
        public static string EmailMessage = "el {PropertyName} No es valido";
        public static bool PrimeraLetraEnMayusculas(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            string Primeraletra = str[0].ToString();

            return Primeraletra == Primeraletra.ToUpper() ? true : false;
        }
    }
}
