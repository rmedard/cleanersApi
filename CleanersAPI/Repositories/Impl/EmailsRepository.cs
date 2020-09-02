using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class EmailsRepository : IEmailsRepository
    
    {
        
        private readonly CleanersApiContext _context;

        public EmailsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Email>> GetAll()
        {
            return await _context.Emails.ToListAsync();
        }

        public Task<Email> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool DoesExist(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Email> Create(Email reservation)
        {
            var newEmail = _context.Emails.Add(reservation).Entity;
            await _context.SaveChangesAsync();
            return newEmail;
        }

        public Task<bool> Update(Email t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}