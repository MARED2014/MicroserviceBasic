using System.ComponentModel;

namespace PizzaPulse.Delivery.Api.Contracts;

public class AssignCourierRequest
{
    public Guid OrderId { get; set; }

    [DefaultValue("Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul")]
    public string CustomerAddress { get; set; } = "Caferağa Mah. Moda Cad. No:12, Kadıköy/İstanbul";
}
