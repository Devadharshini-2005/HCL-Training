
using ComplaintManagementAPI.Data;
using ComplaintManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/complaints
        [HttpPost]
        public async Task<ActionResult<Complaint>> CreateComplaint(
            Complaint complaint)
        {
            complaint.Id = 0;
            complaint.Status = "Open";
            complaint.CreatedAt = DateTime.UtcNow;

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetComplaint),
                new { id = complaint.Id },
                complaint);
        }

        // GET: api/complaints
        // GET: api/complaints?status=Open
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Complaint>>> GetComplaints(
            [FromQuery] string? status)
        {
            IQueryable<Complaint> query = _context.Complaints;

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status != "Open" &&
                    status != "InProgress" &&
                    status != "Resolved")
                {
                    return BadRequest(
                        "Status must be Open, InProgress, or Resolved.");
                }

                query = query.Where(c => c.Status == status);
            }

            return Ok(await query.ToListAsync());
        }

        // GET: api/complaints/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Complaint>> GetComplaint(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
            {
                return NotFound();
            }

            return Ok(complaint);
        }

        // PUT: api/complaints/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComplaint(
            int id, Complaint complaint)
        {
            if (id != complaint.Id && complaint.Id != 0)
            {
                return BadRequest("ID in the URL does not match the body.");
            }

            var existing = await _context.Complaints.FindAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = complaint.Title;
            existing.Description = complaint.Description;
            existing.SubmittedBy = complaint.SubmittedBy;
            existing.Category = complaint.Category;
            existing.Status = complaint.Status;

            // Preserve the original Id and CreatedAt.
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE: api/complaints/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);

            if (complaint == null)
            {
                return NotFound();
            }

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}