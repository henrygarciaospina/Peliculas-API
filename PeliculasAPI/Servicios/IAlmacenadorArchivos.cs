using System.Threading.Tasks;

namespace PeliculasAPI.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta,
            string contentType);

        Task BorrarArchivo(string ruta, string contenido);

        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType);
    }
}
