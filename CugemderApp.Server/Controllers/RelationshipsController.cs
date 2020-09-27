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
    public class RelationshipsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public RelationshipsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Relationships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relationship>>> GetRelationship()
        {
            return await _context.Relationship.ToListAsync();
        }

        // GET: api/Relationships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relationship>> GetRelationship(int id)
        {
            var relationship = await _context.Relationship.FindAsync(id);

            if (relationship == null)
            {
                return NotFound();
            }

            return relationship;
        }

        // PUT: api/Relationships/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelationship(int id, Relationship relationship)
        {
            if (id != relationship.Id)
            {
                return BadRequest();
            }

            _context.Entry(relationship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipExists(id))
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

        // POST: api/Relationships
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Relationship>> PostRelationship(Relationship relationship)
        {
            _context.Relationship.Add(relationship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelationship", new { id = relationship.Id }, relationship);
        }

        // DELETE: api/Relationships/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Relationship>> DeleteRelationship(int id)
        {
            var relationship = await _context.Relationship.FindAsync(id);
            if (relationship == null)
            {
                return NotFound();
            }

            _context.Relationship.Remove(relationship);
            await _context.SaveChangesAsync();

            return relationship;
        }

        private bool RelationshipExists(int id)
        {
            return _context.Relationship.Any(e => e.Id == id);
        }
    }
}
