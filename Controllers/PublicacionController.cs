using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.DTOs.Requests;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmprenderTucumanWebApi.Infrastructure.Repositories;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacionController : ControllerBase
    {
        private PublicacionRepository _publicacionRepository;
        private UsuarioRepository _usuarioRepository;
        private JwtService _jwtService;

        public PublicacionController(PublicacionRepository publicacionRepository,UsuarioRepository usuarioRepository, JwtService jwtService) {

            _publicacionRepository = publicacionRepository;
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        [HttpGet("publicaciones")]
        public async Task<IActionResult> ObtenerPublicaciones() {
            try
            {
                var publicaciones = await _publicacionRepository.GetAllAsync();

                if (publicaciones == null) {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicaciones no encontradas"));
                }

                var publicacionesDto = publicaciones.Select(p => new PublicacionResponseDto { 
                    Id = p.Id,
                    Titulo = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    UrlImagenPrincipal = p.UrlImagenPrincipal
    }).ToList();

                return Ok(ApiResponse<List<PublicacionResponseDto>>.CreateSuccess(publicacionesDto,"publicaciones obtenidas correctamente"));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener las publicaciones: " + ex.Message));

            }

        }

        //[Authorize]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPublicacion([FromBody] PublicacioneRequestDto dto)
        {
            try
            {
                //var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;
                //if (emailUsuario == null) {
                //    return BadRequest(ApiResponse<string>.CreateError("No se encontró el mail"));
                //}
                //    var usuario = await _usuarioRepository.ObtenerUsuarioPorMailAsync(emailUsuario);

                //if (usuario == null)
                //    return NotFound(ApiResponse<string>.CreateNotFound("Usuario no encontrado"));

                var nuevaPublicacion = new Publicacion
                {
                    Nombre = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    CantidadDisponible = 10,
                    FechaPublicacion = DateTime.Now,
                    UrlImagenPrincipal = dto.ImagenUrl,
                    UsuarioId = 2, // o extraído del token
                    CategoriaId = 2,
                    Activa = true
                };


                await _publicacionRepository.AddAsync(nuevaPublicacion);

                return Ok(ApiResponse<string>.CreateSuccess("Publicación creada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno: " + ex.Message));
            }
        }


    }
}
