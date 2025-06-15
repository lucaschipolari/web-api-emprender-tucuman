using EmprenderTucumanWebApi.Custom;
using EmprenderTucumanWebApi.DTOs.Requests;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprenderTucumanWebApi.API;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DBemprendedoresContext _context;
        private readonly JwtService _jwtService;

        public AuthController(DBemprendedoresContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

                if (user == null)
                {
                    return Unauthorized(ApiResponse<string>.CreateError("Correo o contraseña incorrectas"));
                }

                var token = _jwtService.GenerateToken(user);
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
                var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

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

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(newUser);

                return Ok(ApiResponse<string>.CreateSuccess(token, "Usuario registrado con éxito"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno en registro: " + ex.Message));
            }
        }
    }
}
