using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using CleanersAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanersAPI.Repositories.Impl
{
    public class ProfessionalsRepository : IProfessionalsRepository
    {
        private readonly CleanersApiContext _context;

        public ProfessionalsRepository(CleanersApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Professional>> GetAll()
        {
            return await _context.Professionals
                .Include(prof => prof.User)
                .ThenInclude(u => u.Address)
                .Include(prof => prof.Expertises)
                .ThenInclude(e => e.Service)
                .ToListAsync();
        }

        public Task<Professional> GetById(int id)
        {
            return _context.Professionals
                .Include(prof => prof.User)
                .ThenInclude(u => u.Address)
                .Include(p => p.Expertises)
                .SingleOrDefaultAsync(prof => prof.Id.Equals(id));
        }

        public void GrantExpertise(Expertise expertise)
        {
            _context.Expertises.Add(expertise);
            _context.SaveChanges();
        }

        public void UpdateExpertise(Expertise expertise)
        {
            _context.Entry(expertise).State = EntityState.Modified;
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsFree(int professionalId, DateTime dateTime, int numberOfHours)
        {
            var starTime = dateTime;
            var endTime = dateTime.AddHours(numberOfHours);
            return !_context.Reservations
                .Any(r => r.Expertise.ProfessionalId.Equals(professionalId)
                          && DateTime.Compare(r.StartTime, endTime) < 0
                          && DateTime.Compare(r.EndTime, starTime) > 0
                          && Status.Confirmed.Equals(r.Status));
        }

        public async Task<Professional> GetProfessionalByUserId(int userId)
        {
            return await _context.Professionals
                .Include(p => p.User)
                .ThenInclude(u => u.Address)
                .FirstOrDefaultAsync(p => userId.Equals(p.UserId));
        }

        public async Task<IEnumerable<Expertise>> GetExpertises(int professionalId)
        {
            return await _context.Expertises.Include(e => e.Service)
                .Where(e => e.ProfessionalId.Equals(professionalId)).ToListAsync();
        }

        public bool DoesExist(int professionalId)
        {
            return _context.Professionals.Any(e => e.Id == professionalId);
        }

        public async Task<Professional> Create(Professional professional)
        {
            foreach (var expertise in professional.Expertises)
            {
                var service = _context.Services.First(s => expertise.Service.Id.Equals(s.Id));
                expertise.Service = service;
            }
            
            var newProfessional = (await _context.Professionals.AddAsync(professional)).Entity;
            await _context.SaveChangesAsync();
            return newProfessional;
        }

        public async Task<bool> Update(Professional professional)
        {
            _context.Entry(professional).State = EntityState.Modified;
            _context.Entry(professional.User).State = EntityState.Modified;
            _context.Entry(professional.User.Address).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(int professionalId)
        {
            var professional = GetById(professionalId);
            _context.Professionals.Remove(professional.Result);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}