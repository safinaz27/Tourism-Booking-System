using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismApi.Data;
using TourismApi.Data.Models;
using TourismApi.Helper;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext context;

        public BookingController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public string AddBooking(Guid userId, int serviceId)
        {
            context.Booking.Add(new Booking() {userId = userId, serviceId = serviceId, bookingState = BookingState.pending});
            context.SaveChanges();
            return "Booking Succefull";
        }
    }
}
