
using LeaveManagementAPI.Data;
using LeaveManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/LeaveRequests
        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest(
            LeaveRequest request)
        {
            // Validate a supplied status before assigning the initial status.
            if (request.Status != "Pending" &&
                request.Status != "Approved" &&
                request.Status != "Rejected")
            {
                return BadRequest(
                    "Status must be Pending, Approved, or Rejected.");
            }

            request.Id = 0;
            request.Status = "Pending";

            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLeaveRequest),
                new { id = request.Id },
                request);
        }

        // GET: api/LeaveRequests?status=Pending
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>>
            GetLeaveRequests([FromQuery] string? status)
        {
            IQueryable<LeaveRequest> query = _context.LeaveRequests;

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status != "Pending" &&
                    status != "Approved" &&
                    status != "Rejected")
                {
                    return BadRequest(
                        "Status must be Pending, Approved, or Rejected.");
                }

                query = query.Where(r => r.Status == status);
            }

            return Ok(await query.ToListAsync());
        }

        // GET: api/LeaveRequests/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        // PUT: api/LeaveRequests/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLeaveRequest(
            int id, LeaveRequest request)
        {
            if (request.Id != 0 && request.Id != id)
            {
                return BadRequest(
                    "The ID in the URL does not match the body.");
            }

            var existing = await _context.LeaveRequests.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.EmployeeName = request.EmployeeName;
            existing.LeaveType = request.LeaveType;
            existing.StartDate = request.StartDate;
            existing.EndDate = request.EndDate;
            existing.Reason = request.Reason;
            existing.Status = request.Status;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE: api/LeaveRequests/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            _context.LeaveRequests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}