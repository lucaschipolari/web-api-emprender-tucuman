using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IEmprendimientoRepository : IRepository<Emprendimiento> 
    {
        Task<Emprendimiento> ObtenerPorUsuarioIdAsync(int userId);
    }
}
