namespace Backend.Domain.ValueObjects; 

public class OrderName
{
    public string Value { get;}
    
    public OrderName (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentException("Order name cannot be empty");
        Value= value;
    }
}