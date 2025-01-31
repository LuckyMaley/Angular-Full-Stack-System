namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
