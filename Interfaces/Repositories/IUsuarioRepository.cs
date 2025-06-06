using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObtenerUsuarioPorMailAsync(string email);

    }
}
