using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.DTOs.Requests
{
    public class PublicacioneRequestDto
    {
        public string? Titulo { get; set; }

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string? ImagenUrl { get; set; }



    }
}
