using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismApi.Data.Models;
using TourismApi.Data;
using Microsoft.EntityFrameworkCore;
using TourismApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RestaurantsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _db.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        [Authorize(Roles = "RestaurantOwner")]
        [HttpPost("Add_Restaurant")]
        public async Task<IActionResult> AddService([FromForm] mdlRestaurant mdl)
        {
            var ownerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var restaurant = new Restaurant
            {
                Name = mdl.Name,
                Description = mdl.Description,
                Price_per_unite = mdl.Price_per_unite,
                CuisineType = mdl.CuisineType,
                Location = mdl.Location,
                WorkingHours = mdl.WorkingHours,
                Image = stream.ToArray(),
                Menu = stream.ToArray(),
                Availability = "Opened",
                Status = "Pending",
                ServiceOwnerId = ownerId
            };

            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();
            return Ok(restaurant);
        }

        [HttpGet("search_Restaurants")]
        public async Task<IActionResult> SearchRestaurants(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term is required.");
            }

            var services = await _db.Restaurants
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
