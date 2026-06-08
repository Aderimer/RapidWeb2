namespace Backend.Models
{
    public class Merch
    {
        public int MerchId { get; set; }
        required public string ProductName { get; set; }
        required public decimal Price { get; set; }
        required public string Size { get; set; }
        required public Category? Category { get; set; }
        required public int CategoryId { get; set; }
    }
}