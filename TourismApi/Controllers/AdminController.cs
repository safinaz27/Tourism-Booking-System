using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismApi.Data;
using TourismApi.Helper;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("pending-services")]
        public async Task<IActionResult> GetPendingServices()
        {
            var pendingServices = await _db.Booking
                .Where(s => s.bookingState == BookingState.pending)
                .ToListAsync();

            if (!pendingServices.Any())
                return NotFound("No pending services found.");

            return Ok(pendingServices);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("Approve-Service/{id}")]
        public async Task<IActionResult> ApproveService(int id)
        {
            var AdminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var service = await _db.Booking.FindAsync(id);
            if (service == null)
                return NotFound("Service not found.");

            if (service.bookingState != BookingState.pending)
                return BadRequest("Only pending services can be accepted.");
            //service.AdminId = AdminId;
            service.bookingState= BookingState.approved;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = "Service has been approved.",
                service.id,
                service.bookingState
            });
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("Reject-Service/{id}")]
        public async Task<IActionResult> RejectService(int id)
        {
            var AdminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var service = await _db.Booking.FindAsync(id);
            if (service == null)
                return NotFound("Service not found.");

            if (service.bookingState != BookingState.pending)
                return BadRequest("Only pending services can be rejected.");
            //service.AdminId = AdminId;
            service.bookingState = BookingState.rejected;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = "Service has been rejected.",
                service.id,
                service.bookingState
            });
        }
    }
}
