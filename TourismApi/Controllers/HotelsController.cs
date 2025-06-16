using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismApi.Data.Models;
using TourismApi.Data;
using TourismApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public HotelsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _db.Hotels.ToListAsync();
            return Ok(restaurants);
        }

        [Authorize(Roles = "HotelOwner")]
        [HttpPost("Add_Hotel")]
        public async Task<IActionResult> AddService([FromForm] mdlHotel mdl)
        {
            var ownerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var hotel = new Hotel
            {
                Name = mdl.Name,
                Description = mdl.Description,
                Price_per_unite = mdl.Price_per_unite,
                Location = mdl.Location,
                WorkingHours = mdl.WorkingHours,
                Image = stream.ToArray(),
                HasWifi = mdl.HasWifi,
                HasParking = mdl.HasParking,
                RoomView = mdl.RoomView,
                Availability = "Opened",
                Status = "Pending",
                ServiceOwnerId = ownerId
            };

            _db.Hotels.Add(hotel);
            await _db.SaveChangesAsync();
            return Ok(hotel);

            // this is a comment to test the push operation
        }

        [HttpGet("search_Hotels")]
        public async Task<IActionResult> SearchHotels(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term is required.");
            }

            var services = await _db.Hotels
                .Where(s => EF.Functions.Like(s.Name, $"%{name}%"))
                .ToListAsync();

            if (services == null || !services.Any())
            {
                return NotFound("No services matched your search.");
            }

            return Ok(services);
        }

    }
}
