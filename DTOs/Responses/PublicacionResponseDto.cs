using EmprenderTucumanWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EmprenderTucumanWebApi.DTOs.Responses
{
    public class PublicacionResponseDto
    {
        [Required]
        public int Id { get; set; }
        public string? Titulo { get; set; }

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string? UrlImagenPrincipal { get; set; }


    }
}
