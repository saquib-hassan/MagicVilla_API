using MagicVilla_API.Models;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task Create(Villa entity);
        Task Remove(Villa entity);
        Task Save();
        Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> GetVilla(Expression<Func<Villa,bool>> filter = null, bool tracked = true);
    }
}
