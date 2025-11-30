using System.Reflection.Metadata;

namespace Backend.Domain.ValueObjects; 

public class OrderDate
{
    public DateTime Value { get;}
    
    public OrderDate (DateTime value)
    {
        if ( value == DateTime.MinValue)
        throw new ArgumentException("Date cannot be empty");

        if (value > DateTime.Now) 
        throw new ArgumentException ("Order date cannot be in the future");

       Value= value;
    }


}