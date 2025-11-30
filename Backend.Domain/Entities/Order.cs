using Backend.Domain.ValueObjects;

namespace Backend.Domain.Entities; 

public class Order
{
    public Guid Id { get; private set; }
    public OrderName Name { get; private set; }

    public OrderDate Date { get; private set; }

    public decimal Price { get; private set; }

    public Order (OrderName name, decimal price, OrderDate date)
    {
        Id= Guid.NewGuid();
        Name= name;
        Date= date; 
        Price= price;
    }

}