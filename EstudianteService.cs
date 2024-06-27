using Google.Type;
using Newtonsoft.Json;
using Proyecto2Api.DTO;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto2Api
{
    public class EstudianteService
    {
        private readonly HttpClient _httpClient;

        public EstudianteService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<EmailDTO>> ObtenerNotificacionesPendientes()
        {
            try
            {
                // Reemplaza la URL con la URL de tu función de Firebase
                var firebaseFunctionUrl = "https://us-central1-proyect2database.cloudfunctions.net/obtenerNotificacionesNoEnviadas";

                var response = await _httpClient.GetAsync(firebaseFunctionUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var notificaciones = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonResponse);

                var emails = new List<EmailDTO>();
                foreach (var notificacion in notificaciones)
                {
                    var id = notificacion["id"].ToString(); // Convertir el id a string
                    var asunto = notificacion["Asunto"].ToString();
                    var cuerpo = notificacion["Cuerpo"].ToString();
                    var correo = notificacion["Correo"].ToString();

                    emails.Add(new EmailDTO
                    {
                        Id = 0, // Asignar 0 a Id
                        IdFirebase = id,
                        Subject = asunto,
                        Body = cuerpo,
                        AddressTo = correo
                    });
                }

                return emails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> InsertarEstudianteAsync(string nombre, string apellidos, string carne, string correo, string telefono)
        {
            try
            {
                var url = "https://us-central1-proyect2database.cloudfunctions.net/insertarEstudiante";

                // Datos del estudiante a enviar en la solicitud POST
                var requestData = new
                {
                    nombre,
                    apellidos,
                    carne,
                    correo,
                    telefono
                };

                // Convertir datos a formato JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Manejar el error si la respuesta no fue exitosa
                    return $"Error al insertar estudiante. Código de estado: {response.StatusCode}";
                }

                // Leer el mensaje de éxito de la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return $"Error al insertar estudiante: {ex.Message}";
            }
        }

        public async Task<string> ActivarEstudianteAsync(string carne) 
        {
            try
            {
                var url = "https://us-central1-proyect2database.cloudfunctions.net/activarEst";

                // Datos del estudiante a enviar en la solicitud POST
                var requestData = new
                {
                    carne
                };

                // Convertir datos a formato JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await _httpClient.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Manejar el error si la respuesta no fue exitosa
                    return $"Error al desactivar estudiante. Código de estado: {response.StatusCode}";
                }

                // Leer el mensaje de éxito de la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return $"Error al desactivar estudiante: {ex.Message}";
            }
        }

        public async Task<string> DesactivarEstudianteAsync(string carne)
        {
            try
            {
                var url = "https://us-central1-proyect2database.cloudfunctions.net/desactivarEst";

                // Datos del estudiante a enviar en la solicitud POST
                var requestData = new
                {
                    carne
                };

                // Convertir datos a formato JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await _httpClient.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Manejar el error si la respuesta no fue exitosa
                    return $"Error al desactivar estudiante. Código de estado: {response.StatusCode}";
                }

                // Leer el mensaje de éxito de la respuesta
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return $"Error al desactivar estudiante: {ex.Message}";
            }
        }

        public async Task MarcarNotificacionEnviadaAsync(string id)
        {
            try
            {
                var url = "https://us-central1-proyect2database.cloudfunctions.net/marcarNotificacionEnviada";

                // Preparar el cuerpo de la solicitud
                var requestData = new { id };

                // Convertir el cuerpo a JSON
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Manejar el error si la respuesta no fue exitosa
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al llamar la función Firebase: {ex.Message}");
                throw;
            }
        }
    }
}
