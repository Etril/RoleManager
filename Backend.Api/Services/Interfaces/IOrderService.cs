

namespace Backend.Api.Services.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync (CreateOrderDto dto, Guid userId);

    Task<OrderResponseDto> EditOrderAsync (EditOrderDto dto, Guid userId);

    Task<OrderResponseDto> DeleteOrderAsync (Guid orderId, Guid userId );
}