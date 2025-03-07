﻿namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class CustomersOrderDetailsVM
    {
        public int EfUserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string IdentityUsername { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int OrderId { get; set; }
        public int ShippingId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? DeliveryStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
