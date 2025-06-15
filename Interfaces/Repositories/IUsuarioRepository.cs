using EmprenderTucumanWebApi.Models;
using System.Linq.Expressions;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObtenerUsuarioPorMailAsync(string email);

        Task<Usuario?> GetByIdAsync(int id);

        Task<IEnumerable<Usuario>> GetAllAsync();

        Task<Usuario?> FirstOrDefaultAsync(Expression<Func<Usuario, bool>> predicate);

    }
}
