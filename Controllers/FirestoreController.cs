using Microsoft.AspNetCore.Mvc;
using Proyecto2Api.DTO;

namespace Proyecto2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private readonly EstudianteService _estudianteService;
        private readonly Utils _utils;

        public FirestoreController(EstudianteService estudianteService, Utils utils)
        {
            _estudianteService = estudianteService;
            _utils = utils;
        }

        [HttpGet("/GestionarNotificaciones")]
        public async Task<IActionResult> GestionarNotificaciones()
        {
            List<EmailDTO> notificaciones = new List<EmailDTO>();

            notificaciones = await _estudianteService.ObtenerNotificacionesPendientes();
            
            EnviarCorreos(notificaciones);

            return Ok(notificaciones);
        }

        [HttpPost("InsertarEstudiante")]
        public async Task<IActionResult> InsertarEstudianteAsync([FromBody] EstudianteDTO input)
        {
            var result = await _estudianteService.InsertarEstudianteAsync(
                input.Nombre, input.Apellidos, input.Carne, input.Correo, input.Telefono);

            return Ok(result);
        }

        [HttpPut("ActivarEstudiante")]
        public async Task<IActionResult> ActivarEstudiante(string carne)
        {
            var result = await _estudianteService.ActivarEstudianteAsync(carne);

            return Ok(result);
        }

        [HttpPut("DesactivarEstudiante")]
        public async Task<IActionResult> DesactivarEstudiante(string carne)
        {
            var result = await _estudianteService.DesactivarEstudianteAsync(carne);

            return Ok(result);
        }

        private async void EnviarCorreos(List<EmailDTO> notificaciones)
        {
            foreach (EmailDTO notificacion in notificaciones)
            {
                using HttpResponseMessage response = await _utils.GetAPIHost().PostAsJsonAsync(_utils.GetEmailAPI(), notificacion);
                if (response.IsSuccessStatusCode)
                {
                    MarcarNotificacionEnviada(notificacion.IdFirebase);
                }
            }
        }

        private async void MarcarNotificacionEnviada(string idNotificacion)
        {
            await _estudianteService.MarcarNotificacionEnviadaAsync(idNotificacion);
        }
    }
}
