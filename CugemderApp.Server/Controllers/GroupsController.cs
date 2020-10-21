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
    public class GroupsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public GroupsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groups>>> GetGroups()
        {
            return await _context.Groups.ToListAsync();
        }


        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groups>> GetGroups(int id)
        {
            var groups = await _context.Groups.FindAsync(id);

            if (groups == null)
            {
                return NotFound();
            }

            return groups;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroups(int id, Groups groups)
        {
            if (id != groups.Id)
            {
                return BadRequest();
            }

            _context.Entry(groups).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupsExists(id))
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

        // POST: api/Groups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Groups>> PostGroups(Groups groups)
        {
            _context.Groups.Add(groups);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroups", new { id = groups.Id }, groups);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Groups>> DeleteGroups(int id)
        {
            var groups = await _context.Groups.FindAsync(id);
            var users = await _context.AspNetUsers.Where(c => c.Group == id).ToListAsync();
            if (groups == null)
            {
                return NotFound();
            }


            foreach (var user in users)
            {
                user.Group = null;
            }


            await _context.SaveChangesAsync();
            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();

            return groups;
        }

        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
