public record EditOrderCommand (   
    Guid OrderId,
    string Name, 
    decimal Price, 
    DateTime Date
);

public record EditOrderResponse (   
    Guid OrderId,
    string Name, 
    decimal Price, 
    DateTime Date
);