using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : IVillaRepository
    {
        public Task Create(Villa entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<Villa> GetVilla(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Villa entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
