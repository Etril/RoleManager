public class EditOrderDto
{
    public Guid Id {get; set; }

    public string ?Name {get; set;}

    public DateTime DateTime {get; set;}

    public decimal Price {get; set; }
}