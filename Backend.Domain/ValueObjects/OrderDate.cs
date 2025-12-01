using System.Reflection.Metadata;

namespace Backend.Domain.ValueObjects; 

public class OrderDate
{
    public DateTime Value { get;}
    
    public OrderDate (DateTime value)
    {
        if ( value == DateTime.MinValue)
        throw new ArgumentException("Date cannot be empty");

        if (value > DateTime.UtcNow) 
        throw new ArgumentException ("Order date cannot be in the future");

       Value= value;
    }

    public override bool Equals(object? obj) => Equals(obj as OrderDate);
    public bool Equals(OrderDate? other) => other != null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();


}