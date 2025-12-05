using Backend.Domain.Entities;
using Backend.Application.Repositories; // wherever your interface is declared
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context= context;
    }

    public async Task<User?> GetByIdAsync (Guid userId)
    {
        return await _context.Users
        .Include(u => u.Orders)
        .FirstOrDefaultAsync(u=> u.Id == userId);
    }

    public async Task UpdateAsync(User user)
    {
       await _context.SaveChangesAsync();
        
    }

    public async Task<User?> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.Users
        .Include(u => u.Orders )
        .FirstOrDefaultAsync(u=> u.Orders.Any(o=> o.Id==orderId));
        
    }
}