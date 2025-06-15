using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.DTOs.Responses
{
    public class CategoriaResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

    }
}
