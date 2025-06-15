namespace EmprenderTucumanWebApi.DTOs.Requests
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Nombre { get; set; }

    }
}
