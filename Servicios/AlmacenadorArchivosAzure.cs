
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MinimalAPIPeliculas.Servicios
{
    public class AlmacenadorArchivosAzure : IAlmacenadorArchivos
    {
        private string connectionString;

        public AlmacenadorArchivosAzure(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(PublicAccessType.Blob);

            var extension = System.IO.Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(nombreArchivo);
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = archivo.ContentType;
            await blob.UploadAsync(archivo.OpenReadStream(), blobHttpHeaders);
            return blob.Uri.ToString();
        }

        public async Task Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var nombreArchivo = System.IO.Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();
        }
    }
}
