using Backend.Application.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Commands; 

public class EditOrderCommandHandler : IEditOrderCommandHandler
{
    private readonly IUserRepository _userRepository;

    public EditOrderCommandHandler (IUserRepository userRepository)
    {
        _userRepository=userRepository;
        
    }

    public async Task<EditOrderResponse> Handle (EditOrderCommand command, Guid orderId, Guid updatedByUserId)
    {
        var ownerUser = await _userRepository.GetByOrderIdAsync(command.OrderId);
        if (ownerUser == null)
            throw new Exception("Order owner not found");

        var updatedByUser = await _userRepository.GetByIdAsync(updatedByUserId);
        if (updatedByUser == null)
            throw new Exception("User performing update not found");

        ownerUser.UpdateOrderPrice(orderId, command.Price, updatedByUser);

        var updatedOrder= ownerUser.Orders.Single(o => o.Id == orderId);


        await _userRepository.UpdateAsync(ownerUser);

        return new EditOrderResponse(updatedOrder.Id, updatedOrder.Name.Value, updatedOrder.Price, updatedOrder.Date.Value);
}
}