namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class CategoriesProductsVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public float Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime ModifiedDate { get; set; }
		public string? ImageUrl { get; set; }
	}
}
