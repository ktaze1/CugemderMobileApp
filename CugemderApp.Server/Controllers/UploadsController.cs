﻿using System;
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
    public class UploadsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public UploadsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Uploads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Uploads>>> GetUploads()
        {
            return await _context.Uploads.ToListAsync();
        }

        // GET: api/Uploads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Uploads>> GetUploads(int id)
        {
            var uploads = await _context.Uploads.FindAsync(id);

            if (uploads == null)
            {
                return NotFound();
            }

            return uploads;
        }

        // PUT: api/Uploads/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUploads(int id, Uploads uploads)
        {
            if (id != uploads.Id)
            {
                return BadRequest();
            }

            _context.Entry(uploads).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UploadsExists(id))
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

        // POST: api/Uploads
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Uploads>> PostUploads(Uploads uploads)
        {
            _context.Uploads.Add(uploads);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUploads", new { id = uploads.Id }, uploads);
        }

        // DELETE: api/Uploads/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Uploads>> DeleteUploads(int id)
        {
            var uploads = await _context.Uploads.FindAsync(id);
            if (uploads == null)
            {
                return NotFound();
            }

            _context.Uploads.Remove(uploads);
            await _context.SaveChangesAsync();

            return uploads;
        }

        private bool UploadsExists(int id)
        {
            return _context.Uploads.Any(e => e.Id == id);
        }
    }
}
