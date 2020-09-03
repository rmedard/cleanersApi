﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;

namespace CleanersAPI.Repositories
{
    public interface IProfessionalsRepository : ICleanersRepository<Professional>
    {
        Task<IEnumerable<Expertise>> GetExpertises(int professionalId);
        
        void GrantExpertise(Expertise expertise);
        
        void UpdateExpertise(Expertise expertise);
        
        bool IsFree(int professionalId, DateTime dateTime, int numberOfHours);
    }
}
