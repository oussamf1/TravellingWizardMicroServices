using System.Collections.Generic;
using Shared.Data;

namespace Shared.Repos.Interface
{
    public interface IGenericRepo<T> where T : class, IEntity
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
