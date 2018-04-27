using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanersAPI.Services
{
    public interface ICleanersService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetOneById(int id);
        bool DoesExist(int id);
        Task<T> Create(T t);
        Task<T> Update(T t);
        bool Delete(int id);
    }
}