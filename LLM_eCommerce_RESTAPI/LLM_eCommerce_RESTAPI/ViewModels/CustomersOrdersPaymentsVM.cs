namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class CustomersOrdersPaymentsVM
    {
        public int EfUserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string IdentityUsername { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public double PaymentAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public int OrderId { get; set; }
        public int ShippingId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
