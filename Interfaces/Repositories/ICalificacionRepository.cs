using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Interfaces.Repositories
{
    public interface ICalificacionRepository :IRepository<Calificacion>
    {
      //  Task AgregarCalificacionAsync(Calificacion calificacion);
       // Task<IEnumerable<Calificacion>> ObtenerPorPublicacionIdAsync(int publicacionId);
        Task<double> ObtenerPromedioPorPublicacionIdAsync(int publicacionId);
    }

}
