public class EditOrderDto
{
    public Guid Id {get; set; }

    public string ?Name {get; set;}

    public DateTime Date {get; set;}

    public decimal Price {get; set; }
}