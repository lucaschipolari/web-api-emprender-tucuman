using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Models;
using EmprenderTucumanWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaRepository _categoriaRepository;
        private readonly JwtService _jwtService;

        public CategoriaController(CategoriaRepository categoriaRepository, JwtService jwtService)
        {
            _categoriaRepository = categoriaRepository;
            _jwtService = jwtService;
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias() { 
        var categorias = await _categoriaRepository.GetAllAsync();

            if (categorias == null) { 
            
                return NotFound();
            }

            var categoriasDto = categorias.Select(c => new CategoriaResponseDto {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
            }).ToList();

            return Ok(ApiResponse<List<CategoriaResponseDto>>.CreateSuccess(categoriasDto, "Categoriaes obtenidas correctamente"));
        }

    }
}
