public interface IEditOrderCommandHandler
{
    Task<EditOrderResponse> Handle (EditOrderCommand command, Guid orderId, Guid updatedByUserId);
}