using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.DTOs.Responses
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }

        public string? Email { get; set; }

        public string? ImagenPerfil { get; set; }

        public int RolId { get; set; }

        public bool? Activo { get; set; }

    }
}
