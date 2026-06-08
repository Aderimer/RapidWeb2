namespace Backend.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        required public string CategoryName { get; set; }
        public ICollection<Merch>? Merch { get; set; }
    }
}