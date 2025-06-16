using Microsoft.AspNetCore.Identity;

namespace TourismApi.Data.Models
{
    public enum UserRole
        {
        Admin,
        Client,
        RestaurantOwner,
        CarRentalOwner,
        HotelOwner
        }
    public class User:IdentityUser

    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRole UserRole { get; set; }
    }
}
