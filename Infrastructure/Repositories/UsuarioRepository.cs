using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DBemprendedoresContext context) : base(context) { }

        public async Task<Usuario?> ObtenerUsuarioPorMailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }



        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // ✅ Incluir rol
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol) // ✅ Incluir rol
                .ToListAsync();
        }
        public async Task<Usuario?> FirstOrDefaultAsync(Expression<Func<Usuario, bool>> predicate)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(predicate);
        }




    }
}
