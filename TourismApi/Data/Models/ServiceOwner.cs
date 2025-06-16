namespace TourismApi.Data.Models
{
    public class ServiceOwner:User
    {
        public ICollection<Service> services { get; set; }
        public string? ProfilePicture { get; set; }
        public ServiceOwner(string type)
        {
            if (type == "RestaurantOwner")
            {
                UserRole = UserRole.RestaurantOwner;
            }
            if(type == "CarRentalOwner")
            {
                UserRole=UserRole.CarRentalOwner;
            }
            else
            {
                UserRole=UserRole.HotelOwner;
            }
            
        }

        public ServiceOwner()
        {
        }
    }
}
