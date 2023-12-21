namespace cargaDocu.Models
{
    public class Documento
    {

        
        public int IdObra { get; set; }
        public string NombreDocumento { get; set; }
        public string Ruta { get; set; }
        public string TipoDocumento { get; set; }
        public IFormFile Archivo { get; set; }







    }
}
