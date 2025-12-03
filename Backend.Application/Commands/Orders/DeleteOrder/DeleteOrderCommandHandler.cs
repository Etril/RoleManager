using Backend.Application.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Commands;

public class DeleteOrderCommandHandler
{
    private readonly IUserRepository _userRepository;

    public DeleteOrderCommandHandler (IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }

    public async Task<DeleteOrderResponse> Handle (DeleteOrderCommand command, Guid deletedByUserId)
    {
        var ownerUser= await _userRepository.GetByOrderIdAsync(command.OrderId);
        if (ownerUser == null)
        throw new Exception("Order owner not found");

        var deletedByUser= await _userRepository.GetByIdAsync(deletedByUserId);
        if (deletedByUser==null)
        throw new Exception("User performing deletion not found");

        ownerUser.DeleteOrder(command.OrderId, deletedByUser);

        await _userRepository.UpdateAsync(ownerUser);



        return new DeleteOrderResponse("Order deleted", true);



    }
}