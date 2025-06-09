namespace EmprenderTucumanWebApi.DTOs.Requests
{
    public class PublicacionRequestDto
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool EstaEnOferta { get; set; }
        public decimal? PrecioOferta { get; set; }
        public int CantidadDisponible { get; set; }
        public string? ImagenUrl { get; set; }
        public int CategoriaId { get; set; }
        public bool Eliminada { get; set; }

    }


}
