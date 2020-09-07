using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Repositories;

namespace CleanersAPI.Services.Impl
{
    public abstract class CleanersService<T> : ICleanersService<T>
    {
        protected abstract ICleanersRepository<T> GetRepository();
        
        public async Task<IEnumerable<T>> GetAll()
        {
            return await GetRepository().GetAll();
        }

        public Task<T> GetOneById(int id)
        {
            return GetRepository().GetById(id);
        }

        public bool DoesExist(int id)
        {
            return GetRepository().DoesExist(id);
        }

        public Task<T> Create(T t)
        {
            return GetRepository().Create(t);
        }

        public Task<bool> Update(T t)
        {
            return GetRepository().Update(t);
        }

        public Task<bool> Delete(int id)
        {
            return GetRepository().Delete(id);
        }
    }
}