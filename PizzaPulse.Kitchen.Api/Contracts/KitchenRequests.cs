using System.ComponentModel;

namespace PizzaPulse.Kitchen.Api.Contracts;

public class CreateKitchenTaskRequest
{
    public Guid OrderId { get; set; }

    public List<string> ItemsSummary { get; set; } =
    [
        "1x Medium Margherita",
        "2x Large Pepperoni Supreme"
    ];

    [DefaultValue("Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul")]
    public string DeliveryAddress { get; set; } = "Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul";
}
