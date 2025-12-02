using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Backend.Application.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task <Order?> GetbyIdAsync (Guid orderId);

        Task UpdateAsync(Order order);

        Task DeleteAsync(Order order);

        Task <IEnumerable<Order>> GetOrdersAsync (Guid userId);
    }
}