using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto2Api.DTO;

namespace Proyecto2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private readonly FirestoreDb _context;

        public FirestoreController(FireBaseConnection context)
        {
            _context = context.GetFirestoreDb();
        }

        [HttpPost("/EncriptarTexto")]
        public string EncriptarString(string stringAconvertir)
        {
            //encriptar
            string encryptedConnectionString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(stringAconvertir));
            return encryptedConnectionString;
        }

        [HttpGet("read")]
        public async Task<IActionResult> ReadData()
        {
            QuerySnapshot snapshot = await _context.Collection("Estudiante").GetSnapshotAsync();
            var data = snapshot.Documents.Select(d => d.ToDictionary()).ToList();
            return Ok(data);
        }

        [HttpPost("InsertarEstudiante")]
        public async Task<IActionResult> InsertarEstudiante(EstudianteDTO input)
        {
            try
            {
                await _context.RunTransactionAsync(async transaction =>
                {
                    
                    CollectionReference estudiantesRef = _context.Collection("Estudiante");

                    Dictionary<string, object> estudiante = new Dictionary<string, object>
                    {
                        { "Nombre", input.Nombre },
                        { "Apellidos", input.Apellidos },
                        { "Carne", input.Carne },
                        { "Correo", input.Correo },
                        { "Telefono", input.Telefono },
                        { "Activo", false }
                    };

                    DocumentReference newEstudianteRef = estudiantesRef.Document();
                    transaction.Create(newEstudianteRef, estudiante);
                });

                return Ok("Estudiante creado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud. "+ex);
            }
        }

        [HttpPut("ActivarEstudiante")]
        public async Task<IActionResult> ActualizarEstadoActivo(string carne)
        {
            try
            {
                CollectionReference estudiantesRef = _context.Collection("Estudiante");

                QuerySnapshot querySnapshot = await estudiantesRef.WhereEqualTo("Carne", carne).GetSnapshotAsync();
                DocumentSnapshot documentSnapshot = querySnapshot.Documents.FirstOrDefault();

                if (documentSnapshot != null)
                {
                    string idDocumento = documentSnapshot.Id;

                    Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "Activo", true }
                };

                    await estudiantesRef.Document(idDocumento).SetAsync(updates, SetOptions.MergeAll);

                    return Ok($"Estado Activo actualizado a true para el estudiante con carne: {carne}");
                }
                else
                {
                    return NotFound($"No se encontró ningún estudiante con carne: {carne}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado Activo del estudiante: {ex.Message}");
            }
        }
    }
}
