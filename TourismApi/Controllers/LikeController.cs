using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismApi.Data.Models;
using TourismApi.Data;
using Microsoft.EntityFrameworkCore;
using TourismApi.Models;

namespace TourismApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikesController(AppDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Likes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLikeById(int id)
        {
            var like = await _context.Likes
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (like == null)
            {
                return NotFound();
            }
            return Ok(like);
        }
        // GET: api/Likes/service/
        [HttpGet("service/{serviceId}")]
        public async Task<ActionResult<Like>> GetLikesByService(int serviceId)
        {
            var likes = await _context.Likes
                .Where(l => l.ServiceId == serviceId)
                .Include(l => l.User)
                .ToListAsync();
            if (likes == null)  
            {
                return NotFound();
            }
            return Ok(likes);
        }

        // GET: api/Likes/client/
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<Like>> GetLikesByClient(string clientId)
        {
            var likes = await _context.Likes
                .Where(l => l.ClientId == clientId)
                .Include(l => l.User)
                .ToListAsync();
            if (likes == null)
            {
                return NotFound();
            }
            return Ok(likes);
        }

        // POST: api/Likes
        [HttpPost] 
        public async Task<ActionResult<Like>> CreateLike(LikeCreateDto likeDto)
        {
            var serviceExists = await _context.Services.AnyAsync(s => s.Id == likeDto.ServiceId);
            if (!serviceExists)
            {
                return BadRequest("Invalid service specified.");
            }

            var like = new Like
            {
                ServiceId = likeDto.ServiceId,
                ClientId = likeDto.ClientId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLikeById), new { id = like.Id }, like);
        }
        // GET: api/Likes/check/service/5/client/abc123
        [HttpGet("check/service/{serviceId}/client/{clientId}")]
        public async Task<ActionResult<bool>> CheckIfLiked(int serviceId, string clientId)
        {
            var exists = await _context.Likes
                .AnyAsync(l => l.ServiceId == serviceId && l.ClientId == clientId);
            
            return exists;
        }
        // GET: api/Likes/count/service/5
        [HttpGet("count/service/{serviceId}")]
        public async Task<ActionResult<int>> GetLikesCountForService(int serviceId)
        {
            return await _context.Likes
                .CountAsync(l => l.ServiceId == serviceId);
        }
        // GET: api/Likes/count/client/abc123
        [HttpGet("count/client/{clientId}")]            
        public async Task<ActionResult<int>> GetLikesCountForClient(string clientId)
        {
            return await _context.Likes
                .CountAsync(l => l.ClientId == clientId);
        }
        // DELETE: api/Likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}