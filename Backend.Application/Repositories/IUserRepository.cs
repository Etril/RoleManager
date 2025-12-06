using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync (Guid userId);
        Task UpdateAsync (User user);

        Task<User?> GetByOrderIdAsync(Guid orderId);

        Task<User?> GetByUsernameAsync (UserName username);
    }
}