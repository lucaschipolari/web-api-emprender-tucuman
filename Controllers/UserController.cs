using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.DTOs.Requests;

using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UserController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly RolRepository _rolRepository;
        private readonly JwtService _jwtService;

        public UserController(UsuarioRepository usuarioRepository,RolRepository rolRepository, JwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _rolRepository = rolRepository;
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
                    Id = u.Id,
                    Email = u.Email,
                    Nombre = u.NombreUsuario,
                    ImagenPerfil = u.ImagenPerfil,
                    RolId = u.RolId,
                    Activo = u.Activo,
                }).ToList();

                return Ok(ApiResponse<List<UsuarioResponseDto>>.CreateSuccess(usuariosDto, "Usuarios obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener los usuarios: " + ex.Message));
            }
        }

        [Authorize(Policy = "Administradores")]
        [HttpPut("usuarios/{id}/rol")]
        public async Task<IActionResult> CambiarRol(int id, [FromBody] CambiarRolDto dto)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound(ApiResponse<string>.CreateNotFound("Usuario no encontrado"));

                var rol = await _rolRepository.GetByIdAsync(dto.RolId);
                if (rol == null || !rol.Activo)
                    return BadRequest(ApiResponse<string>.CreateError("Rol inválido"));

                usuario.RolId = dto.RolId;
                await _usuarioRepository.UpdateAsync(usuario);

                return Ok(ApiResponse<string>.CreateSuccess("Rol actualizado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError($"Error: {ex.Message}"));
            }
        }

       
     
        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesDisponibles()
        {
            try
            {
                var roles = await _rolRepository.GetActivosAsync();
                return Ok(ApiResponse<List<Role>>.CreateSuccess(roles.ToList()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError($"Error: {ex.Message}"));
            }
        }

        [Authorize(Policy = "Administradores")]
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
        [Authorize(Policy = "Administradores")]
        [HttpPut("usuarios/{id}/desactivar")]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Usuario no encontrado"));
                }
                if (usuario.Activo == true)
                {
                    usuario.Activo = false;
                }
                else {
                    usuario.Activo = true;
                }

                await _usuarioRepository.UpdateAsync(usuario);

                return Ok(ApiResponse<string>.CreateSuccess("Usuario eliminado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al eliminar el usuario: " + ex.Message));
            }
        }
    }
}
