namespace PizzaPulse.Delivery.Api.Contracts;

public class AssignCourierRequest
{
    public Guid OrderId { get; set; }
    public string CustomerAddress { get; set; } = string.Empty;
}

public class CreateCourierRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class UpdateCourierRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string VehiclePlate { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
