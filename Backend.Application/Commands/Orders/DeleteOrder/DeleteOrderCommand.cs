public record DeleteOrderCommand(Guid OrderId); 

public record DeleteOrderResponse
(
    string Message, 

    bool Success

);