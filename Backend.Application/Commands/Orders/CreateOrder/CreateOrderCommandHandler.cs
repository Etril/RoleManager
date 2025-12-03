using Backend.Application.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Commands;

public class CreateOrderCommandHandler
{
    private readonly IUserRepository _userRepository;

    public CreateOrderCommandHandler(IUserRepository userRepository)
    {
        _userRepository= userRepository;
    }

    public async Task<CreateOrderResponse> Handle (CreateOrderCommand command, Guid targetUserId, Guid createdByUserId)
    {
        var ownerUser = await _userRepository.GetByIdAsync(targetUserId);
        if (ownerUser==null)
        {
            throw new Exception("User not found");
        }
        
        var createdByUser = await _userRepository.GetByIdAsync(createdByUserId);
        if (createdByUser==null)
        {
            throw new Exception("User not found");
        }


        var order = new Order(
            new OrderName(command.Name),
            command.Price, 
            new OrderDate(command.Date)
        );

        ownerUser.AddOrder(order, createdByUser);

        await _userRepository.UpdateAsync(ownerUser);

        return new CreateOrderResponse(order.Id, order.Name.Value, order.Price, order.Date.Value);
    }


};