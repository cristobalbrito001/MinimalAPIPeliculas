namespace MinimalAPIPeliculas.Entidades
{
    public class Error
    {
        public Guid Id { get; set; }
        public string? MessageDeError { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Fecha { get; set; }
    }
}
