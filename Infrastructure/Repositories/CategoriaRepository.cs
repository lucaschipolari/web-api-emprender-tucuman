using EmprenderTucumanWebApi.Models;

namespace EmprenderTucumanWebApi.Infrastructure.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria> 
    {
        public CategoriaRepository(DBemprendedoresContext context) : base(context)
        {
        }

    }
}
