using Backend.Application.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Commands;

public class CreateOrderCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository; 

    public CreateOrderCommandHandler(IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _userRepository= userRepository;
        _orderRepository= orderRepository;
    }

    public async Task<CreateOrderResponse> Handle (CreateOrderCommand command, Guid userId)
    {
        var user= await _userRepository.GetByIdAsync(userId);
        if (user==null)
        {
            throw new Exception("User not found");
        }

        var order = new Order(
            new OrderName(command.Name),
            command.Price, 
            new OrderDate(command.Date)
        );

        await _orderRepository.AddAsync(order);

        return new CreateOrderResponse(order.Id, order.Name.Value, order.Price, order.Date.Value);
    }


};