using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface IPublicacionRespository : IRepository<Publicacion>
    {
        Task<IEnumerable<Publicacion>> GetPublicacionRangeAsync(decimal precioMinimo, decimal precioMaximo);

        Task<IEnumerable<Publicacion>> GetPublicacionesSinPausar();

    }
}
