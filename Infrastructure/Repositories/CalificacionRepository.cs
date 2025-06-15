using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class CalificacionRepository : GenericRepository<Calificacion>, ICalificacionRepository
    {
        public CalificacionRepository(DBemprendedoresContext context) : base(context)
        {
        }

        public async Task<double> ObtenerPromedioPorPublicacionIdAsync(int publicacionId)
        {
            var calificaciones = await _context.Calificacions
                .Where(c => c.PublicacionId == publicacionId)
                .ToListAsync();

            if (!calificaciones.Any())
                return 0;

            return calificaciones.Average(c => c.Puntuacion);
        }
    }
}
