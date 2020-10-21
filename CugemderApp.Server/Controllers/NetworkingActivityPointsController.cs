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
    public class NetworkingActivityPointsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public NetworkingActivityPointsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/NetworkingActivityPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NetworkingActivityPoint>>> GetNetworkingActivityPoint()
        {
            return await _context.NetworkingActivityPoint.ToListAsync();
        }

        // GET: api/NetworkingActivityPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NetworkingActivityPoint>> GetNetworkingActivityPoint(int id)
        {
            var networkingActivityPoint = await _context.NetworkingActivityPoint.FindAsync(id);

            if (networkingActivityPoint == null)
            {
                return NotFound();
            }

            return networkingActivityPoint;
        }

        // PUT: api/NetworkingActivityPoints/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetworkingActivityPoint(int id, NetworkingActivityPoint networkingActivityPoint)
        {
            if (id != networkingActivityPoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(networkingActivityPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkingActivityPointExists(id))
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

        // POST: api/NetworkingActivityPoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<NetworkingActivityPoint>> PostNetworkingActivityPoint(NetworkingActivityPoint networkingActivityPoint)
        {
            _context.NetworkingActivityPoint.Add(networkingActivityPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNetworkingActivityPoint", new { id = networkingActivityPoint.Id }, networkingActivityPoint);
        }

        // DELETE: api/NetworkingActivityPoints/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NetworkingActivityPoint>> DeleteNetworkingActivityPoint(int id)
        {
            var networkingActivityPoint = await _context.NetworkingActivityPoint.FindAsync(id);
            if (networkingActivityPoint == null)
            {
                return NotFound();
            }

            _context.NetworkingActivityPoint.Remove(networkingActivityPoint);
            await _context.SaveChangesAsync();

            return networkingActivityPoint;
        }

        private bool NetworkingActivityPointExists(int id)
        {
            return _context.NetworkingActivityPoint.Any(e => e.Id == id);
        }
    }
}
