using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientoController : ControllerBase
    {
        private readonly IEmprendimientoRepository _emprendimientoRepository;
        public EmprendimientoController(IEmprendimientoRepository emprendimientoRepository) {
            
            _emprendimientoRepository = emprendimientoRepository;
        }


        
        [HttpGet("emprendedor")]
        public async Task<IActionResult> ObtenerEmprendedorDelUsuarioActual()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var emprendimiento = await _emprendimientoRepository.ObtenerPorUsuarioIdAsync(userId);

                if (emprendimiento == null)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("El usuario no tiene emprendimientos."));
                }

                var dto = new EmprendimientoResponseDto
                {
                    Id = emprendimiento.Id,
                    Nombre = emprendimiento.Nombre,
                    Direccion = emprendimiento.Direccion,
                    Descripcion = emprendimiento.Descripcion,
                    WhatsApp = emprendimiento.WhatsApp,
                    Instagram = emprendimiento.Instagram,
                    Facebook = emprendimiento.Facebook,
                    Portada = emprendimiento.FotoPortada,
                    FotoPerfil = emprendimiento.FotoPerfil,
                    EmailUsuario = emprendimiento.Usuario.Email
                };

                return Ok(ApiResponse<EmprendimientoResponseDto>.CreateSuccess(dto, "Perfil del emprendedor obtenido correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno: " + ex.Message));
            }
        }


    }
}
