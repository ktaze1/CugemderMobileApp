using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CugemderApp.Shared.Models;

namespace CugemderApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingPointsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public MeetingPointsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/MeetingPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingPoints>>> GetMeetingPoints()
        {
            return await _context.MeetingPoints.ToListAsync();
        }

        // GET: api/MeetingPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingPoints>> GetMeetingPoints(int id)
        {
            var meetingPoints = await _context.MeetingPoints.FindAsync(id);

            if (meetingPoints == null)
            {
                return NotFound();
            }

            return meetingPoints;
        }

        // PUT: api/MeetingPoints/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingPoints(int id, MeetingPoints meetingPoints)
        {
            if (id != meetingPoints.Id)
            {
                return BadRequest();
            }

            _context.Entry(meetingPoints).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingPointsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MeetingPoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MeetingPoints>> PostMeetingPoints(MeetingPoints meetingPoints)
        {
            _context.MeetingPoints.Add(meetingPoints);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetingPoints", new { id = meetingPoints.Id }, meetingPoints);
        }

        // DELETE: api/MeetingPoints/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MeetingPoints>> DeleteMeetingPoints(string id)
        {

            var user = _context.AspNetUsers.FirstOrDefault(c => c.Id == id);
            var meetingPoints = await _context.MeetingPoints.Where(c => c.ReceiverUserId == id).ToListAsync();
            if (meetingPoints == null)
            {
                return NotFound();
            }

            foreach (var item in meetingPoints)
            {
                _context.MeetingPoints.Remove(item);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeetingPointsExists(int id)
        {
            return _context.MeetingPoints.Any(e => e.Id == id);
        }
    }
}
