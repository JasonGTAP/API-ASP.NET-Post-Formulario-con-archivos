using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using cargaDocu.Models;

namespace cargaDocu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {

        private readonly string _rutaServidor;
        private readonly string _cadenaSql;



        public DocumentoController(IConfiguration config) {
            _rutaServidor = config.GetSection("Configuracion").GetSection("RutaServidor").Value;
            _cadenaSql = config.GetConnectionString("CadenaSql");
        
        
        }

        [HttpPost]
        [Route("subir")]


        public IActionResult Subir([FromForm] Documento request) {


            string rutaDocumento = Path.Combine(_rutaServidor, request.Archivo.FileName);

            try
            {
                using(FileStream newFile = System.IO.File.Create(rutaDocumento))
                {
                    request.Archivo.CopyTo(newFile);
                    newFile.Flush();   


                }

                using(var conexion = new SqlConnection(_cadenaSql))
                {


                    conexion.Open();

                    var cmd = new SqlCommand("sp_guardar_documentos", conexion);
                    cmd.Parameters.AddWithValue("nombreDocumento", request.NombreDocumento);
                    cmd.Parameters.AddWithValue("ruta", rutaDocumento);
                    cmd.Parameters.AddWithValue("idObra", request.IdObra);
                    cmd.Parameters.AddWithValue("tipoDocumento", request.TipoDocumento);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();  
                    
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Documento Guardado" });




            }catch (Exception error)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Documento no Guardado" + error });


            }



        
        
        
        }















    }
}
