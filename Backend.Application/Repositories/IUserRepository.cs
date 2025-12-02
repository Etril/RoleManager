using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync (Guid userId);
        Task UpdateAsync (User user);
    }
}