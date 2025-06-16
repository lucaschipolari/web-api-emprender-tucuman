using EmprenderTucumanWebApi.Custom;
using EmprenderTucumanWebApi.DTOs.Requests;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Infrastructure.Repositories;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IRolRepository _rolRepository;

        public AuthController(IUsuarioRepository context,IRolRepository rolRepository, JwtService jwtService)
        {
            _userRepository = context;
            _jwtService = jwtService;
            _rolRepository = rolRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                var user = await _userRepository
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

                if (user == null)
                {
                    return Unauthorized(ApiResponse<string>.CreateError("Correo o contraseña incorrectas"));
                }
                if (user.Rol == null)
                    throw new Exception("El rol no está cargado");

                var token = _jwtService.GenerateToken(user, user.Rol.Nivel);
                return Ok(ApiResponse<string>.CreateSuccess(token, "Inicio de sesión exitoso"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno en login: " + ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            try
            {
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == registerDto.Email);


                if (existingUser != null)
                {
                    return BadRequest(ApiResponse<string>.CreateError("Usuario ya existente"));
                }

                var newUser = new Usuario
                {
                    NombreUsuario = registerDto.Nombre,
                    Email = registerDto.Email,
                    PasswordHash = registerDto.Password
                };

               await _userRepository.AddAsync(newUser);
               
                newUser.Rol = await _rolRepository.GetByIdAsync(newUser.RolId);

                if (newUser.Rol == null)
                    throw new Exception("El rol no está cargado");

                var token = _jwtService.GenerateToken(newUser,newUser.Rol.Nivel);

                return Ok(ApiResponse<string>.CreateSuccess(token, "Usuario registrado con éxito"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno en registro: " + ex.Message));
            }
        }
    }
}
