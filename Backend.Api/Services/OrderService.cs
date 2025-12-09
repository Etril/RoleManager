using Backend.Api.Services.Interfaces;
using Backend.Application.Commands;
using Backend.Application.Repositories;
namespace Backend.Api.Services.Order;

public class OrderService : IOrderService
{

    private readonly CreateOrderCommandHandler _createOrderHandler;

    private readonly EditOrderCommandHandler _editOrderHandler;

    private readonly DeleteOrderCommandHandler _deleteOrderHandler;

    public OrderService (CreateOrderCommandHandler createOrderHandler, EditOrderCommandHandler editOrderHandler, DeleteOrderCommandHandler deleteOrderHandler)
    {
        _createOrderHandler = createOrderHandler;

        _editOrderHandler = editOrderHandler;

        _deleteOrderHandler = deleteOrderHandler;
    }

    public async Task <CreateOrderResponse> CreateOrderAsync (CreateOrderDto dto, Guid userId, Guid targetUserId)
    {
        var command = new CreateOrderCommand(dto.Name!, dto.Price, dto.Date);
        if (dto.Name == null)
        throw  new Exception("Name cannot be empty");

        return await _createOrderHandler.Handle(command, userId, targetUserId);
    }

    public async Task <EditOrderResponse> EditOrderAsync (EditOrderDto dto, Guid orderId, Guid userId )
    {
        var command = new EditOrderCommand(orderId, dto.Name!, dto.Price, dto.Date);
        if (dto.Name == null)
        throw new Exception("Name cannot be empty");

        return await _editOrderHandler.Handle(command, orderId, userId);
    }

    public async Task <DeleteOrderResponse> DeleteOrderAsync (Guid orderId, Guid userId)
    {
        var command= new DeleteOrderCommand(orderId);

        return await _deleteOrderHandler.Handle(command, userId);
    }
}
