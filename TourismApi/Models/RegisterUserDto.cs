using System.ComponentModel.DataAnnotations;
using TourismApi.Data.Models;

namespace MoviesApi.Dto
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public UserRole UserRole { get; set; }

    }
}
