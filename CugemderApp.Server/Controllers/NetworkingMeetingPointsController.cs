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
    public class NetworkingMeetingPointsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public NetworkingMeetingPointsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/NetworkingMeetingPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkingMeetingPoints>>> GetNetworkingMeetingPoints()
        {
            return await _context.NetworkingMeetingPoints.ToListAsync();
        }

        // GET: api/NetworkingMeetingPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NetworkingMeetingPoints>> GetNetworkingMeetingPoints(int id)
        {
            var networkingMeetingPoints = await _context.NetworkingMeetingPoints.FindAsync(id);

            if (networkingMeetingPoints == null)
            {
                return NotFound();
            }

            return networkingMeetingPoints;
        }

        // PUT: api/NetworkingMeetingPoints/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkingMeetingPoints(int id, NetworkingMeetingPoints networkingMeetingPoints)
        {
            if (id != networkingMeetingPoints.Id)
            {
                return BadRequest();
            }

            _context.Entry(networkingMeetingPoints).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkingMeetingPointsExists(id))
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

        // POST: api/NetworkingMeetingPoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NetworkingMeetingPoints>> PostNetworkingMeetingPoints(NetworkingMeetingPoints networkingMeetingPoints)
        {
            _context.NetworkingMeetingPoints.Add(networkingMeetingPoints);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNetworkingMeetingPoints", new { id = networkingMeetingPoints.Id }, networkingMeetingPoints);
        }

        // DELETE: api/NetworkingMeetingPoints/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NetworkingMeetingPoints>> DeleteNetworkingMeetingPoints(int id)
        {
            var networkingMeetingPoints = await _context.NetworkingMeetingPoints.FindAsync(id);
            if (networkingMeetingPoints == null)
            {
                return NotFound();
            }

            _context.NetworkingMeetingPoints.Remove(networkingMeetingPoints);
            await _context.SaveChangesAsync();

            return networkingMeetingPoints;
        }

        private bool NetworkingMeetingPointsExists(int id)
        {
            return _context.NetworkingMeetingPoints.Any(e => e.Id == id);
        }
    }
}
