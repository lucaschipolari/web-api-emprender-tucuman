using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {

        public UsuarioRepository(DBemprendedoresContext context) : base(context)
        {
        }

        public async Task<Usuario?> ObtenerUsuarioPorMailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        }


    }
}


