using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismApi.Data;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var services = await _db.Services.ToListAsync();
            return Ok(services);
        }

        [HttpGet("Approved-services")]
        public async Task<IActionResult> GetPendingServices()
        {
            var pendingServices = await _db.Services
                .Where(s => s.Status == "Approved")
                .ToListAsync();

            if (!pendingServices.Any())
                return NotFound("No pending services found.");

            return Ok(pendingServices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> AllServices(int id)
        {
            var service = await _db.Services.SingleOrDefaultAsync(x => x.Id == id);
            if (service == null)
            {
                return NotFound("Servuce Not Found");
            }
            return Ok(service);
        }

        [HttpGet("Search_ALL")]
        public async Task<IActionResult> SearchServices(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term is required.");
            }

            var services = await _db.Services
                .Where(s => EF.Functions.Like(s.Name, $"%{name}%"))
                .ToListAsync();

            if (services == null || !services.Any())
            {
                return NotFound("No services matched your search.");
            }

            return Ok(services);
        }


        [HttpDelete("id")]
        public async Task<IActionResult> RemoveService(int id)
        {
            var c = await _db.Services.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Service not found ");
            }
            _db.Services.Remove(c);
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpPut("Change-availability/{id}")]
        public async Task<IActionResult> ChangeAvailability(int id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            service.Availability = service.Availability == "Opened" ? "Closed" : "Opened";

            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Service availability changed to {service.Availability}.",
                service.Id,
                service.Availability
            });
        }


    }
}
