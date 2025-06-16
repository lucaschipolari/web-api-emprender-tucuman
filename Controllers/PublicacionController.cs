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
using EmprenderTucumanWebApi.Interfaces.Repositories;

namespace EmprenderTucumanWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PublicacionController : ControllerBase
    {
        private readonly PublicacionRepository _publicacionRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly CalificacionRepository _calificacionRepository;
        private readonly EmprendimientoRepository _emprendimientoRepository;
        private readonly JwtService _jwtService;

        public PublicacionController(
            PublicacionRepository publicacionRepository,
            UsuarioRepository usuarioRepository,
            CalificacionRepository calificacionRepository,
            EmprendimientoRepository emprendimientoRepository,
            JwtService jwtService)
        {
            _publicacionRepository = publicacionRepository;
            _usuarioRepository = usuarioRepository;
            _calificacionRepository = calificacionRepository;
            _emprendimientoRepository = emprendimientoRepository;
            _jwtService = jwtService;
        }

        private async Task<Emprendimiento?> ObtenerEmprendimientoActualAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return null;

            return await _emprendimientoRepository.ObtenerPorUsuarioIdAsync(userId);
        }


        [HttpGet("publicaciones/emprendedor")]
        public async Task<IActionResult> ObtenerPublicacionesPorEmprendedor([FromQuery] int emprendimientoId)
        {
            try
            {
                var publicaciones = await _publicacionRepository.GetPublicacionesActivasPorEmprendimientoAsync(emprendimientoId);

                if (publicaciones == null || !publicaciones.Any())
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("No se encontraron publicaciones para este emprendimiento."));
                }

                var publicacionesDto = new List<PublicacionResponseDto>();

                foreach (var p in publicaciones)
                {
                    var calificacionPromedio = await _calificacionRepository.ObtenerPromedioPorPublicacionIdAsync(p.Id);

                    publicacionesDto.Add(new PublicacionResponseDto
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
                        ComentariosCantidad = p.Comentarios?.Count ?? 0,
                        CalificacionPromedio = calificacionPromedio,
                        Emprendedor =  null
                    });
                }

                return Ok(ApiResponse<List<PublicacionResponseDto>>.CreateSuccess(publicacionesDto, "Publicaciones del emprendedor obtenidas correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener las publicaciones: " + ex.Message));
            }
        }

        [HttpGet("publicaciones")]
        public async Task<IActionResult> ObtenerPublicacionesSinPausar()
        {
            try
            {
                var publicaciones = await _publicacionRepository.GetPublicacionesSinPausar();
                if (publicaciones == null || !publicaciones.Any())
                {
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicaciones no encontradas"));
                }

                var publicacionesDto = new List<PublicacionResponseDto>();

                foreach (var p in publicaciones.Where(p => p.Eliminada == false))
                {
                    var calificacionPromedio = await _calificacionRepository.ObtenerPromedioPorPublicacionIdAsync(p.Id);

                    publicacionesDto.Add(new PublicacionResponseDto
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
                        ComentariosCantidad = p.Comentarios?.Count ?? 0,
                        CalificacionPromedio = calificacionPromedio,

                        Emprendedor = p.Emprendimiento != null
                            ? new EmprendimientoResponseDto
                            {
                                Id = p.Emprendimiento.Id,
                                Nombre = p.Emprendimiento.Nombre,
                                FotoPerfil = p.Emprendimiento.FotoPerfil ?? "", 
                                Direccion = p.Emprendimiento.Direccion ?? "",
                                Instagram = p.Emprendimiento.Instagram ?? "",
                                Facebook = p.Emprendimiento.Facebook ?? "",
                                WhatsApp = p.Emprendimiento.WhatsApp ?? "",
                                Descripcion = p.Emprendimiento.Descripcion ?? "",
                                NombreUsuario = p.Emprendimiento.Usuario?.Nombre ?? "",
                                EmailUsuario = p.Emprendimiento.Usuario?.Email ?? ""
                            }
                            : null
                    });
                }

                return Ok(ApiResponse<List<PublicacionResponseDto>>.CreateSuccess(publicacionesDto, "Publicaciones obtenidas correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno al obtener las publicaciones: " + ex.Message));
            }
        }
        
        [Authorize(Policy = "Emprendedores")]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPublicacion([FromBody] PublicacionRequestDto dto)
        {
            try
            {
                var emprendimiento = await ObtenerEmprendimientoActualAsync();
                if (emprendimiento == null)
                    return NotFound(ApiResponse<string>.CreateNotFound("No se encontró un emprendimiento asociado a este usuario."));

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
                    CategoriaId = dto.CategoriaId,
                    Activa = true,
                    Eliminada = false,
                    EmprendimientoId = emprendimiento.Id
                };

                await _publicacionRepository.AddAsync(nuevaPublicacion);
                return Ok(ApiResponse<string>.CreateSuccess("Publicación creada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error interno: " + ex.Message));
            }
        }

        [Authorize(Policy = "Emprendedores")]
        [HttpPut("pausar/{id}")]
        public async Task<IActionResult> PausarPublicacion(int id)
        {
            try
            {
                var publicacion = await _publicacionRepository.GetByIdAsync(id);
                if (publicacion == null || publicacion.Eliminada == true)
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada o eliminada"));

                var emprendimiento = await ObtenerEmprendimientoActualAsync();
                if (emprendimiento == null)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes un emprendimiento asociado."));

                if (publicacion.EmprendimientoId != emprendimiento.Id)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes permiso para pausar esta publicación."));

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

        [Authorize(Policy = "Emprendedores")]
        [HttpPut("eliminar/{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            try
            {
                var publicacion = await _publicacionRepository.GetByIdAsync(id);
                if (publicacion == null || publicacion.Eliminada == true)
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada o ya eliminada"));

                var emprendimiento = await ObtenerEmprendimientoActualAsync();
                if (emprendimiento == null)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes un emprendimiento asociado."));

                if (publicacion.EmprendimientoId != emprendimiento.Id)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes permiso para eliminar esta publicación."));

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

        [Authorize(Policy = "Emprendedores")]
        [HttpGet("mis-publicaciones")]
        public async Task<IActionResult> ObtenerMisPublicaciones()
        {
            try
            {
                var emprendimiento = await ObtenerEmprendimientoActualAsync();
                if (emprendimiento == null)
                    return NotFound(ApiResponse<string>.CreateNotFound("No se encontró un emprendimiento asociado a este usuario."));

                var publicaciones = await _publicacionRepository.GetByEmprendimientoIdAsync(emprendimiento.Id);

                var publicacionesDto = new List<PublicacionResponseDto>();

                foreach (var p in publicaciones.Where(p => p.Eliminada == false))
                {
                    var calificacionPromedio = await _calificacionRepository.ObtenerPromedioPorPublicacionIdAsync(p.Id);

                    publicacionesDto.Add(new PublicacionResponseDto
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
                        ComentariosCantidad = p.Comentarios?.Count ?? 0,
                        CalificacionPromedio = calificacionPromedio,

                        Emprendedor = p.Emprendimiento != null
                            ? new EmprendimientoResponseDto
                            {
                                Id = p.Emprendimiento.Id,
                                Nombre = p.Emprendimiento.Nombre,
                                FotoPerfil = p.Emprendimiento.FotoPerfil ?? "",
                                Direccion = p.Emprendimiento.Direccion ?? "",
                                Instagram = p.Emprendimiento.Instagram ?? "",
                                Facebook = p.Emprendimiento.Facebook ?? "",
                                WhatsApp = p.Emprendimiento.WhatsApp ?? "",
                                Descripcion = p.Emprendimiento.Descripcion ?? "",
                                NombreUsuario = p.Emprendimiento.Usuario?.Nombre ?? "",
                                EmailUsuario = p.Emprendimiento.Usuario?.Email ?? ""
                            }
                            : null
                    });
                }

                return Ok(ApiResponse<List<PublicacionResponseDto>>.CreateSuccess(publicacionesDto, "Publicaciones obtenidas correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error al obtener publicaciones del emprendedor: " + ex.Message));
            }
        }

        [Authorize(Policy = "Emprendedores")]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditarPublicacion(int id, [FromBody] PublicacionRequestDto dto)
        {
            try
            {
                var publicacion = await _publicacionRepository.GetByIdAsync(id);
                if (publicacion == null || publicacion.Eliminada == true)
                    return NotFound(ApiResponse<string>.CreateNotFound("Publicación no encontrada o eliminada"));

                var emprendimiento = await ObtenerEmprendimientoActualAsync();
                if (emprendimiento == null)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes un emprendimiento asociado."));

                if (publicacion.EmprendimientoId != emprendimiento.Id)
                    return Unauthorized(ApiResponse<string>.CreateError("No tienes permiso para editar esta publicación."));

                // Actualizar propiedades
                publicacion.Nombre = dto.Titulo;
                publicacion.Descripcion = dto.Descripcion;
                publicacion.Precio = dto.Precio;
                publicacion.CategoriaId = dto.CategoriaId;
                publicacion.UrlImagenPrincipal = dto.ImagenUrl;
                publicacion.EstaEnOferta = dto.EstaEnOferta;
                publicacion.PrecioOferta = dto.PrecioOferta;
                publicacion.CantidadDisponible = dto.CantidadDisponible;
                publicacion.UltimaActualizacion = DateTime.Now;

                await _publicacionRepository.UpdateAsync(publicacion);
                return Ok(ApiResponse<string>.CreateSuccess("Publicación editada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.CreateError("Error al editar publicación: " + ex.Message));
            }
        }


    }

}
