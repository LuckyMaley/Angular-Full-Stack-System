namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class PaymentsVM
    {
        public string? PaymentMethod { get; set; }
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string? Status { get; set; }
    }
}
