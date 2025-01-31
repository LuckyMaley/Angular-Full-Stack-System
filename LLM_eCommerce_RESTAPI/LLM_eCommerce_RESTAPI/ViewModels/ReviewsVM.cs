namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class ReviewsVM
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
    }
}
