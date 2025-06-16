namespace TourismApi.Models
{
    // DTOs for Feedback
    
    // This DTO is used when creating a new feedback
    public class FeedbackCreateDto
    {
        public int ServiceId { get; set; }
        public string ClientId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    // This DTO is used when returning feedback data        
    public class FeedbackDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ClientId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}