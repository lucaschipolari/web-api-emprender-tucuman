using Azure.Core;
using EmprenderTucumanWebApi.DTOs.Responses;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class RolRepository : GenericRepository<Role> , IRolRepository
    {
        public RolRepository(DBemprendedoresContext context): base(context) {
        }

        public async Task<List<Role>> GetByNombreAsync(IEnumerable<string> nombres)
        {
            return await _context.Roles
                .Where(r => nombres.Contains(r.Nombre))
                .ToListAsync();
        }
        public async Task<IEnumerable<Role>> GetActivosAsync()
        {
            return await _context.Roles
                .Where(r => r.Activo)
                .OrderBy(r => r.Nivel)
                .ToListAsync();
        }
    }
}
