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

        if (price<0) 
        throw new ArgumentException("Price can't be negative");

        Id= Guid.NewGuid();
        Name= name;
        Date= date; 
        Price= price;
    }

     /* public void UpdatePrice (decimal newPrice, User updatedBy)
    {
        if (newPrice < 0) 
        throw new ArgumentException ("Price can't be negative");

        if (!updatedBy.Role.CanUpdatePrice())
        throw new UnauthorizedAccessException("You are not allowed to update the price");
    } */

}