using EmprenderTucumanWebApi.API;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/rol")]
    public class RolesController : ControllerBase
    {
        private readonly IRolRepository _rolRepository;

        public RolesController(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _rolRepository.GetAllAsync();

                if (roles == null || !roles.Any())
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Roles no encontrados"));
                }

                var rolesDto = roles.Select(r => new RolResponseDto
                {
                    Id = r.Id,
                    Nombre = r.Nombre
                }).ToList();

                return Ok(ApiResponse<List<RolResponseDto>>.CreateSuccess(rolesDto, "Roles obtenidos correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener los roles: " + ex.Message));
            }
        }

    }
}
