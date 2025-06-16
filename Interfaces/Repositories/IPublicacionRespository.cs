using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IPublicacionRespository : IRepository<Publicacion>
    {
        Task<IEnumerable<Publicacion>> GetPublicacionRangeAsync(decimal precioMinimo, decimal precioMaximo);

        Task<IEnumerable<Publicacion>> GetPublicacionesSinPausar();

        Task<IEnumerable<Publicacion>> GetByEmprendimientoIdAsync(int emprendimientoId);

        Task<List<Publicacion>> GetPublicacionesActivasPorEmprendimientoAsync(int emprendimientoId);

    }
}
