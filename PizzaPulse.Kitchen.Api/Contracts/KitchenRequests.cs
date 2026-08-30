namespace PizzaPulse.Kitchen.Api.Contracts;

public class CreateKitchenTaskRequest
{
    public Guid OrderId { get; set; }
    public List<string> ItemsSummary { get; set; } = [];
    public string DeliveryAddress { get; set; } = string.Empty;
}
