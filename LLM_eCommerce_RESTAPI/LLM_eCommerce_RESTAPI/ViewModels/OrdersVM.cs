namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class OrdersVM
    {
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public double TotalAmount { get; set; }
    }
}
