namespace Backend.Models
{
    public class Event
    {
        public int EventId { get; set; }
        required public string Title { get; set; }
        required public string Description { get; set; }
        required public string ImageUrl { get; set; }
        required public DateTime Date { get; set; }
        public string? Location { get; set; }
    }
}