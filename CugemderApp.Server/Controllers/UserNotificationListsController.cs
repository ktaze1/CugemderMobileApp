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
    public class UserNotificationListsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;

        public UserNotificationListsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserNotificationLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserNotificationList>>> GetUserNotificationList()
        {
            return await _context.UserNotificationList.ToListAsync();
        }

        // GET: api/UserNotificationLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserNotificationList>> GetUserNotificationList(int id)
        {
            var userNotificationList = await _context.UserNotificationList.FindAsync(id);

            if (userNotificationList == null)
            {
                return NotFound();
            }

            return userNotificationList;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<ActionResult<List<UserNotificationList>>> GetUserNotificationListOfUser(string id)
        {
            var userNotificationList = await _context.UserNotificationList.Where(c => c.Receiver == id).OrderByDescending(c => c.Date).ToListAsync();

            if (userNotificationList == null)
            {
                return NotFound();
            }

            return userNotificationList;
        }

        // PUT: api/UserNotificationLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserNotificationList(int id, UserNotificationList userNotificationList)
        {
            if (id != userNotificationList.Id)
            {
                return BadRequest();
            }

            _context.Entry(userNotificationList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserNotificationListExists(id))
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

        // POST: api/UserNotificationLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserNotificationList>> PostUserNotificationList(UserNotificationList userNotificationList)
        {
            _context.UserNotificationList.Add(userNotificationList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserNotificationList", new { id = userNotificationList.Id }, userNotificationList);
        }

        // DELETE: api/UserNotificationLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserNotificationList>> DeleteUserNotificationList(int id)
        {
            var userNotificationList = await _context.UserNotificationList.FindAsync(id);
            if (userNotificationList == null)
            {
                return NotFound();
            }

            _context.UserNotificationList.Remove(userNotificationList);
            await _context.SaveChangesAsync();

            return userNotificationList;
        }

        private bool UserNotificationListExists(int id)
        {
            return _context.UserNotificationList.Any(e => e.Id == id);
        }
    }
}
