using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly JwtService _jwtService;

        public UserController(UsuarioRepository usuarioRepository, JwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var usuarios = await _usuarioRepository.GetAllAsync();

                if (usuarios == null)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Usuarios no encontrados"));
                }

                var usuariosDto = usuarios.Select(u => new UsuarioResponseDto
                {
                    Email = u.Email,
                    Nombre = u.Nombre,
                }).ToList();

                return Ok(ApiResponse<List<UsuarioResponseDto>>.CreateSuccess(usuariosDto, "Usuarios obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener los usuarios: " + ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Usuario no encontrado"));
                }

                await _usuarioRepository.DeleteAsync(id);

                return Ok(ApiResponse<string>.CreateSuccess("Usuario eliminado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al eliminar el usuario: " + ex.Message));
            }
        }
    }
}
