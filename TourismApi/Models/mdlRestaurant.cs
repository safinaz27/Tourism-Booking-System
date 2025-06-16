namespace TourismApi.Models
{
    public class mdlRestaurant : mdlService
    {
        public IFormFile Menu { get; set; }
        public string CuisineType { get; set; }
    }
}
