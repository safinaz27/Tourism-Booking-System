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
    public class CarsController : ControllerBase
    {


        private readonly AppDbContext _db;

        public CarsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _db.Cars.ToListAsync();
            return Ok(cars);
        }

        [Authorize(Roles = "CarRentalOwner")]
        [HttpPost("Add_CarRental")]
        public async Task<IActionResult> AddService([FromForm] mdlCar mdl)
        {
            var ownerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var car = new CarRental
            {
                Name = mdl.Name,
                Description = mdl.Description,
                Price_per_unite = mdl.Price_per_unite,
                Location = mdl.Location,
                WorkingHours = mdl.WorkingHours,
                Image = stream.ToArray(),
                CarType = mdl.CarType,
                RentalPeriod = mdl.RentalPeriod,
                Availability = "Opened",
                Status = "Pending",
                ServiceOwnerId = ownerId
            };

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
            return Ok(car);
        }

        [HttpGet("search_CarRentals")]
        public async Task<IActionResult> SearchCarRentals(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term is required.");
            }

            var services = await _db.Cars
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
