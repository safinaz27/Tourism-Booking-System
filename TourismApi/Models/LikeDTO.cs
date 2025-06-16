namespace TourismApi.Models
{
    public class LikeCreateDto
    {
        public int ServiceId { get; set; }
        public string ClientId { get; set; }
    }
    public class LikeDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
    } 
}