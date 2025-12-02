public record CreateOrderCommand (
    string Name, 
    decimal Price, 
    DateTime Date
);

public record CreateOrderResponse (
    Guid OrderId, 
    string Name, 
    decimal Price,
    DateTime Date

);