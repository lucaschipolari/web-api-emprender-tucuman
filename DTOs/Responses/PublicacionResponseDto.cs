using EmprenderTucumanWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EmprenderTucumanWebApi.DTOs.Responses
{
    public class PublicacionResponseDto
    {
        [Required]
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public decimal? PrecioOferta { get; set; }

        public bool EstaEnOferta { get; set; }

        public int CantidadDisponible { get; set; }

        public bool Activa { get; set; }

        public bool Eliminada { get; set; } 

        public string? UrlImagenPrincipal { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        public string? CategoriaNombre { get; set; }

        public int CategoriaId { get; set; }
        public double? CalificacionPromedio { get; set; } 
        public int ComentariosCantidad { get; set; }
        public EmprendimientoResponseDto? Emprendedor { get; set; }

    }

}
