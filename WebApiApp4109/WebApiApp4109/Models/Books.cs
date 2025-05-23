namespace WebApiApp4109.Models
{
    public class Books
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public String ISBN { get; set; }
        public int YearPublished { get; set; }
        public String Publisher { get; set; }
        public String Category { get; set; }
        public String ShelfLocation { get; set; }
    }
}
