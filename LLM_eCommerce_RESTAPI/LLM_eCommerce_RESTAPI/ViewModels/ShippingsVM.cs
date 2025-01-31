namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class ShippingsVM
    {

		public DateTime ShippingDate { get; set; }
		public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? DeliveryStatus { get; set; }
    }
}
