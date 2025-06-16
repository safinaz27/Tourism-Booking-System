using TourismApi.Helper;

namespace TourismApi.Data.Models
{
    public class Booking
    {
        public int id { get; set; }
        public Guid userId { get; set; }
        public int serviceId { get; set; }

        public User user { get; set; }
        public Service service{ get; set; }
        public BookingState bookingState { get; set; }
    }

}
