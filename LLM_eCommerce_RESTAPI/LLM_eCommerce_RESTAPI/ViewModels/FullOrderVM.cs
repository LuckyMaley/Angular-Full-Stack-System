namespace LLM_eCommerce_RESTAPI.ViewModels
{
	public class FullOrderVM
	{
		public string? ShippingAddress { get; set; }
		public string? ShippingMethod { get; set; }
		public double TotalAmount { get; set; }
		public string? PaymentMethod { get; set; }
		public string? Status { get; set; }

		public List<OrderDetailsTwoVM> OrderDetails { get; set; } = new List<OrderDetailsTwoVM>();
	}
}
