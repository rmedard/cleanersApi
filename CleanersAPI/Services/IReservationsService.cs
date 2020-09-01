﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CleanersAPI.Helpers;
using CleanersAPI.Models;

namespace CleanersAPI.Services
{
    public interface IReservationsService : ICleanersService<Reservation>
    {
        Task<IEnumerable<Reservation>> searchByProfessionalByStatus(Professional professional, Status status);
    }
}