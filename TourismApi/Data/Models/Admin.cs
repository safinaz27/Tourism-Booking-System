using Microsoft.AspNetCore.Identity;

namespace TourismApi.Data.Models
{
    public class Admin : User
    {
        public string? ProfilePicture { get; set; }
        public Admin()
        {
            UserRole = UserRole.Admin;
        }
        public ICollection<Service> Services { get; set; }
    }

}