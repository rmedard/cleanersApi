using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface ICleanersRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        bool DoesExist(int id);
        Task<T> Create(T t);
        Task<bool> Update(T t);
        Task<bool> Delete(int id);
    }
}