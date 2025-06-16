using System.ComponentModel.DataAnnotations;

namespace TourismApi.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Price_per_unite { get; set; }
        public string WorkingHours { get; set; }
        public string Status { get; set; }
        public string Availability { get; set; }
        public byte[]? Image { get; set; }
        public double TotalRating { get; set; }

        public string ServiceOwnerId { get; set; }
        public ServiceOwner ServiceOwner { get; set; }
        public string AdminId { get; set; }
        public Admin Admin { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Like> Likes { get; set; } 

    }
}
