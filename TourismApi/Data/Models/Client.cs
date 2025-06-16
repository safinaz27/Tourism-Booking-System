namespace TourismApi.Data.Models
{
    public class Client:User
    {
        public string Address { get; set; }
        public string? ProfilePicture { get; set; }
        
        
        public Client()
        {
            UserRole = UserRole.Client;
        }
    }
}
