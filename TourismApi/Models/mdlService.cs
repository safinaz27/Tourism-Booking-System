namespace TourismApi.Models
{
    public class mdlService
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string Location { get; set; }
        public decimal Price_per_unite { get; set; }
        public string WorkingHours { get; set; }
    }
}
