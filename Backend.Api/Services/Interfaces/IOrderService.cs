

namespace Backend.Api.Services.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResponse> CreateOrderAsync (CreateOrderDto dto, Guid userId, Guid targetUserId);

    Task<EditOrderResponse> EditOrderAsync (EditOrderDto dto, Guid orderId, Guid userId);

    Task<DeleteOrderResponse> DeleteOrderAsync (Guid orderId, Guid userId );
}