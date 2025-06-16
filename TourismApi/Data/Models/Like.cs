using TourismApi.Data.Models;

public class Like
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public string ClientId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public Client User { get; set; }
    public Service Service { get; set; }
}