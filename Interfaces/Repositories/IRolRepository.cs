using Azure.Core;
using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IRolRepository : IRepository<Role>
    {

        Task<List<Role>> GetByNombreAsync(IEnumerable<string> nombres);
        Task<IEnumerable<Role>> GetActivosAsync();
    }
}
