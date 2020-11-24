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
    public class JobReferencesController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public JobReferencesController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/JobReferences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobReferences>>> GetJobReferences()
        {
            return await _context.JobReferences.ToListAsync();
        }

        // GET: api/JobReferences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobReferences>> GetJobReferences(int id)
        {
            var jobReferences = await _context.JobReferences.FindAsync(id);

            if (jobReferences == null)
            {
                return NotFound();
            }

            return jobReferences;
        }

        [HttpGet]
        [Route("referencer/{id}")]
        public async Task<ActionResult<List<JobReferences>>> GetJobReferencesReferencer(string id)
        {
            var jobReferences = await _context.JobReferences.Where(c => c.ReferencerId == id).ToListAsync();

            if (jobReferences == null)
            {
                return NotFound();
            }

            return jobReferences;
        }

        [HttpGet]
        [Route("referenced/{id}")]
        public async Task<ActionResult<List<JobReferences>>> GetJobReferencesReferenced(string id)
        {
            var jobReferences = await _context.JobReferences.Where(c => c.ReferencedId == id).ToListAsync();

            if (jobReferences == null)
            {
                return NotFound();
            }

            return jobReferences;
        }



        // PUT: api/JobReferences/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobReferences(int id, JobReferences jobReferences)
        {
            if (id != jobReferences.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobReferences).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobReferencesExists(id))
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

        // POST: api/JobReferences
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<JobReferences>> PostJobReferences(JobReferences jobReferences)
        {
            _context.JobReferences.Add(jobReferences);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobReferences", new { id = jobReferences.Id }, jobReferences);
        }

        // DELETE: api/JobReferences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobReferences>> DeleteJobReferences(int id)
        {
            var jobReferences = await _context.JobReferences.FindAsync(id);
            if (jobReferences == null)
            {
                return NotFound();
            }

            _context.JobReferences.Remove(jobReferences);
            await _context.SaveChangesAsync();

            return jobReferences;
        }

        private bool JobReferencesExists(int id)
        {
            return _context.JobReferences.Any(e => e.Id == id);
        }
    }
}
