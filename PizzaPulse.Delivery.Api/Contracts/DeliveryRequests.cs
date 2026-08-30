namespace PizzaPulse.Delivery.Api.Contracts;

public class AssignCourierRequest
{
    public Guid OrderId { get; set; }
    public string CustomerAddress { get; set; } = string.Empty;
}
