using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismApi.Data.Models;
using TourismApi.Data;
using Microsoft.EntityFrameworkCore;
using TourismApi.Models;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }
    
    // Helper function to update service rating
    private async Task UpdateServiceRating(int serviceId)
    {
        var service = await _context.Services.FindAsync(serviceId);
        if (service == null)
        {
            return;
        }

        var feedbacks = await _context.Feedbacks
            .Where(f => f.ServiceId == serviceId)
            .ToListAsync();

        int RatingCount = feedbacks.Count;
        int SumOfRatings = feedbacks.Sum(f => f.Rating);
        
        if (RatingCount > 0)
        {
            service.TotalRating = (double)SumOfRatings / RatingCount;
        }
        else
        {
            service.TotalRating = 0;
        }

        await _context.SaveChangesAsync();
    }
    // Create new feedback
    [HttpPost]      
    public async Task<ActionResult<Feedback>> CreateFeedback(FeedbackCreateDto feedbackDto)
    {
        if (feedbackDto.Rating < 1 || feedbackDto.Rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }

        var serviceExists = await _context.Services.AnyAsync(s => s.Id == feedbackDto.ServiceId);
        if (!serviceExists)
        {
            return BadRequest("Invalid service specified.");
        }

        var feedback = new Feedback
        {
            ServiceId = feedbackDto.ServiceId,
            ClientId = feedbackDto.ClientId,
            Rating = feedbackDto.Rating,
            Comment = feedbackDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();
        
        // Update service rating
        await UpdateServiceRating(feedback.ServiceId);

        return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.Id }, feedback);
    }
    // Get feedback by ID       
    [HttpGet("{id}")]               
    public async Task<ActionResult<Feedback>> GetFeedbackById(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
        {
            return NotFound("Feedback not found.");
        }
        return Ok(feedback);
    }                                                                                                                    
    // Get all feedback for a service
    [HttpGet("service/{serviceId}")]
    public async Task<ActionResult<Feedback>> GetFeedbackByService(int serviceId)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.ServiceId == serviceId)
            .ToListAsync();
        return Ok(feedbacks);
    }

    // Get all feedback for a client
    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<Feedback>> GetFeedbackByUser(string ClientId)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.ClientId == ClientId)
            .ToListAsync();
        return Ok(feedbacks);
    }
    // Update existing feedback
    [HttpPut("{id}")]
    public async Task<ActionResult<Feedback>> UpdateFeedback(int id, FeedbackCreateDto feedbackDto)
    {
        if (feedbackDto.Rating < 1 || feedbackDto.Rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5.");
        }

        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
        {
            return NotFound("Feedback not found.");
        }

        feedback.Rating = feedbackDto.Rating;
        feedback.Comment = feedbackDto.Comment;
        feedback.CreatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        // Update service rating after modification
        await UpdateServiceRating(feedback.ServiceId);

        return Ok(feedback);
    }
    // Delete feedback
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFeedback(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null)
        {
            return NotFound("Feedback not found.");
        }

        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();
        
        // Update service rating after deletion
        await UpdateServiceRating(feedback.ServiceId);

        return NoContent();
    }
}