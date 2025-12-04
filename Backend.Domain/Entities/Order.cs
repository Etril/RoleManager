using Backend.Domain.ValueObjects;

namespace Backend.Domain.Entities; 

public class Order

{

    private Order() { }
    public Guid Id { get; private set; }
    public OrderName Name { get; private set; } =default!;

    public OrderDate Date { get; private set; } = default!;

    public decimal Price { get; private set; }

    public Order (OrderName name, decimal price, OrderDate date)
    {

        if (price<0) 
        throw new ArgumentException("Price can't be negative");

        Id= Guid.NewGuid();
        Name= name;
        Date= date; 
        Price= price;
    }

    public void UpdatePrice(decimal newPrice)
{
    if (newPrice < 0)
        throw new ArgumentException("Price cannot be negative");

    Price = newPrice;
}


}