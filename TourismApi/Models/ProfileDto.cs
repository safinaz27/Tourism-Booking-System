namespace TourismApi.Models
{
    public class ProfileDto
    {

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfilePicture { get; set; }


    }
}
