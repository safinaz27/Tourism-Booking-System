using TourismApi.Data.Models;

public class Feedback
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public int ServiceId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Client Client { get; set; }
    public Service Service { get; set; }
}