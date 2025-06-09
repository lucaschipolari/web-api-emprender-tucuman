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
        private readonly PublicacionRepository _publicacionRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly JwtService _jwtService;

        public PublicacionController(
            PublicacionRepository publicacionRepository,
            UsuarioRepository usuarioRepository,
            JwtService jwtService)
        {
            _publicacionRepository = publicacionRepository;
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        [HttpGet("publicaciones")]
        public async Task<IActionResult> ObtenerPublicaciones()
        {
            try
            {
                var publicaciones = await _publicacionRepository.GetAllAsync();

                if (publicaciones == null || !publicaciones.Any())
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicaciones no encontradas"));
                }

                var publicacionesDto = publicaciones.Where(p => p.Eliminada==false).Select(p => new PublicacionResponseDto
                {
                    Id = p.Id,
                    Titulo = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    PrecioOferta = p.PrecioOferta,
                    EstaEnOferta = p.EstaEnOferta ?? false,
                    CantidadDisponible = p.CantidadDisponible,
                    Activa = p.Activa ?? true,
                    UrlImagenPrincipal = p.UrlImagenPrincipal,
                    FechaPublicacion = p.FechaPublicacion,
                    CategoriaId = p.CategoriaId,
                    ComentariosCantidad = p.Comentarios?.Count ?? 0
                }).ToList();

                return Ok(ApiResponse<List<PublicacionResponseDto>>.CreateSuccess(publicacionesDto, "Publicaciones obtenidas correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener las publicaciones: " + ex.Message));
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPublicacion([FromBody] PublicacionRequestDto dto)
        {
            try
            {
                // 🛡️ Si usás autenticación JWT, obtené el usuario desde el token:
                // var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;
                // var usuario = await _usuarioRepository.ObtenerUsuarioPorMailAsync(emailUsuario);
                // if (usuario == null)
                //     return NotFound(ApiResponse<string>.CreateNotFound("Usuario no encontrado"));

                var nuevaPublicacion = new Publicacion
                {
                    Nombre = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    EstaEnOferta = dto.EstaEnOferta,
                    PrecioOferta = dto.PrecioOferta,
                    CantidadDisponible = dto.CantidadDisponible,
                    FechaPublicacion = DateTime.Now,
                    UrlImagenPrincipal = dto.ImagenUrl,
                    UsuarioId = 2, // O usuario.Id si tomás el usuario logueado
                    CategoriaId = dto.CategoriaId,
                    Activa = true,
                    Eliminada = true,
                };

                await _publicacionRepository.AddAsync(nuevaPublicacion);

                return Ok(ApiResponse<string>.CreateSuccess("Publicación creada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno: " + ex.Message));
            }
        }

        [HttpPut("pausar/{id}")]
        public async Task<IActionResult> PausarPublicacion(int id)
        {
            try
            {
                var publicacion = await _publicacionRepository.GetByIdAsync(id);
                if (publicacion == null || publicacion.Eliminada == true)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada"));
                }

                publicacion.Activa = !(publicacion.Activa ?? true);
                publicacion.UltimaActualizacion = DateTime.Now;

                await _publicacionRepository.UpdateAsync(publicacion);

                string estado = publicacion.Activa == true ? "activada" : "pausada";
                return Ok(ApiResponse<string>.CreateSuccess($"Publicación {estado} correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error al pausar publicación: " + ex.Message));
            }
        }
        [HttpPut("eliminar/{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            try
            {
                var publicacion = await _publicacionRepository.GetByIdAsync(id);
                if (publicacion == null || publicacion.Eliminada == true)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada o ya eliminada"));
                }

                publicacion.Eliminada = true;
                publicacion.UltimaActualizacion = DateTime.Now;

                await _publicacionRepository.UpdateAsync(publicacion);

                return Ok(ApiResponse<string>.CreateSuccess("Publicación eliminada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error al eliminar publicación: " + ex.Message));
            }
        }
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditarPublicacion(int id, [FromBody] PublicacionRequestDto dto)
        {
            try
            {
                var publicacionExistente = await _publicacionRepository.GetByIdAsync(id);
                if (publicacionExistente == null || publicacionExistente.Eliminada == true)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada o eliminada"));
                }

                // Actualizar propiedades
                publicacionExistente.Nombre = dto.Titulo;
                publicacionExistente.Descripcion = dto.Descripcion;
                publicacionExistente.Precio = dto.Precio;
                publicacionExistente.UrlImagenPrincipal = dto.ImagenUrl;
                publicacionExistente.EstaEnOferta = dto.EstaEnOferta;
                publicacionExistente.PrecioOferta = dto.PrecioOferta;
                publicacionExistente.CantidadDisponible = dto.CantidadDisponible;
                // Agrega aquí todas las propiedades que quieras actualizar

                await _publicacionRepository.UpdateAsync(publicacionExistente);
                return Ok(ApiResponse<string>.CreateSuccess("Publicación editada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error al editar publicación: " + ex.Message));
            }
        }


    }

}
