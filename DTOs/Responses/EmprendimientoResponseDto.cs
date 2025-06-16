namespace EmprenderTucumanWebApi.DTOs.Responses
{
    public class EmprendimientoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Portada { get; set; }
        public string? Direccion { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WhatsApp { get; set; }
        public string? Descripcion { get; set; }
        public string? NombreUsuario { get; set; }
        public string? EmailUsuario { get; set; }

    }
}