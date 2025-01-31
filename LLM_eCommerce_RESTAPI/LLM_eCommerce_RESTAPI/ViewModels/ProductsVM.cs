namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class ProductsVM
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public float Price { get; set; } = 0;
        public int CategoryId { get; set; } = 0;
        public int StockQuantity { get; set; } = 0;

		public string? ImageUrl { get; set; }

	}
}
