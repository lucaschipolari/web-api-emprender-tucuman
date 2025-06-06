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


    }
}
