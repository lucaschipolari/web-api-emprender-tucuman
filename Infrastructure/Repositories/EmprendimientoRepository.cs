using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class EmprendimientoRepository : GenericRepository<Emprendimiento>,IEmprendimientoRepository
    {
       public EmprendimientoRepository(DBemprendedoresContext context) :base(context) { 
        
        }

        public async Task<Emprendimiento?> ObtenerPorUsuarioIdAsync(int userId) {

            return await _context.Emprendimientos
    .Include(e => e.Usuario).FirstOrDefaultAsync(e => e.UsuarioId == userId && e.Estado);

        }
    }
}
