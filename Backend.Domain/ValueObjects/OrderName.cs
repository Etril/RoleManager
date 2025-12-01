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

    public override bool Equals(object? obj) => Equals(obj as OrderName);
    public bool Equals(OrderName? other) => other != null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}