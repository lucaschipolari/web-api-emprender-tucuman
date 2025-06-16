using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class PublicacionRepository : GenericRepository<Publicacion>, IPublicacionRespository
    {
        public PublicacionRepository(DBemprendedoresContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Publicacion>> GetPublicacionRangeAsync(decimal precioMinimo, decimal precioMaximo) {

            return await _context.Publicacions.Where(p => p.Precio>=precioMinimo && p.Precio<=precioMaximo).ToListAsync();

        }
        public async Task<IEnumerable<Publicacion>> GetPublicacionesSinPausar()
        {
            return await _context.Publicacions
                .Include(p => p.Emprendimiento)
                    .ThenInclude(e => e.Usuario) 
                .Include(p => p.Categoria) 
                .Where(p => p.Activa == true && !p.Eliminada)
                .ToListAsync();
        }

        public async Task<List<Publicacion>> GetAllAsync()
        {
            return await _context.Publicacions
                .Include(p => p.Usuario) // importante: relación debe estar bien configurada
                .Include(p => p.Categoria) // si también usás CategoriaNombre
                .ToListAsync();
        }
        public async Task<List<Publicacion>> GetPublicacionesActivasPorEmprendimientoAsync(int emprendimientoId)
        {
            return await _context.Publicacions
                .Include(p => p.Emprendimiento)
                    .ThenInclude(e => e.Usuario)
                .Include(p => p.Categoria)
                .Where(p => p.Activa == true && !p.Eliminada && p.EmprendimientoId == emprendimientoId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Publicacion>> GetByEmprendimientoIdAsync(int emprendimientoId)
        {
            return await _context.Publicacions
                .Include(p => p.Emprendimiento)
                    .ThenInclude(e => e.Usuario)
                .Include(p => p.Comentarios)
                .Where(p => p.EmprendimientoId == emprendimientoId)
                .ToListAsync();
        }
    }
}
