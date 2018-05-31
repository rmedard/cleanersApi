﻿using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;

namespace CleanersAPI.Services
{
    public interface IAuthService
    {
        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
        string GenerateLoginToken(User user);
        void AddUserToProfessional(Professional professional, UserForLoginDto userForLoginDto);
        void AddUserToCustomer(Customer customer, UserForLoginDto userForLoginDto);
        Task<User> GetUserById(int userId);
    }
}