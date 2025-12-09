public interface IDeleteOrderCommandHandler
{
    Task<DeleteOrderResponse> Handle (DeleteOrderCommand command, Guid deletedByUserId);
}