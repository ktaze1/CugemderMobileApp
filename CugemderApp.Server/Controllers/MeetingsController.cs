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
    public class MeetingsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public MeetingsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meetings>>> GetMeetings()
        {
            return await _context.Meetings.ToListAsync();
        }

        [HttpGet]
        [Route("user/points/{id}")]
        public async Task<ActionResult<IEnumerable<Meetings>>> GetMeetingsNeedPoint(string id)
        {
            var meetings = await _context.Meetings
                .Where(c => (c.SenderId == id && c.IsResultedbySender == false) || (c.ReceiverId == id && c.IsResultedbyReceiver == false))
                .Where(c => c.Date.AddHours(2) < DateTime.Now)
                .ToListAsync();
            var returnMeetings = meetings.ToList();
            var users = await _context.AspNetUsers.ToListAsync();
            List<string> userIds = new List<string>();
            foreach (var item in users)
            {
                userIds.Add(item.Id);
            }
            foreach (var item in meetings)
            {
                if(!userIds.Contains(item.SenderId) || !userIds.Contains(item.ReceiverId))
                {
                    returnMeetings.Remove(item);
                }
            }

            return returnMeetings;
        }

        // GET: api/Meetings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meetings>> GetMeetings(int id)
        {
            var meetings = await _context.Meetings.FindAsync(id);

            if (meetings == null)
            {
                return NotFound();
            }

            return meetings;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<ActionResult<IEnumerable<Meetings>>> GetMeetingsOfUser(string id)
        {
            var meetings = await _context.Meetings
                .Where(c => c.ReceiverId == id || c.SenderId == id).ToListAsync();

            if (meetings == null)
            {
                return NotFound();
            }

            var returnMeetings = meetings.ToList();
            var users = await _context.AspNetUsers.ToListAsync();
            List<string> userIds = new List<string>();
            foreach (var item in users)
            {
                userIds.Add(item.Id);
            }
            foreach (var item in meetings)
            {
                if (!userIds.Contains(item.SenderId) || !userIds.Contains(item.ReceiverId))
                {
                    returnMeetings.Remove(item);
                }
            }

            return returnMeetings;
        }

        // PUT: api/Meetings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetings(int id, Meetings meetings)
        {
            if (id != meetings.Id)
            {
                return BadRequest();
            }

            _context.Entry(meetings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingsExists(id))
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

        // POST: api/Meetings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Meetings>> PostMeetings(Meetings meetings)
        {
            _context.Meetings.Add(meetings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetings", new { id = meetings.Id }, meetings);
        }

        // DELETE: api/Meetings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Meetings>> DeleteMeetings(int id)
        {
            var meetings = await _context.Meetings.FindAsync(id);
            if (meetings == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meetings);
            await _context.SaveChangesAsync();

            return meetings;
        }

        private bool MeetingsExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}
