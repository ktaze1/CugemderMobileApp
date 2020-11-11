﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CugemderApp.Shared.Models;
using CugemderApp.Server.Model;
using Microsoft.AspNetCore.Identity;
using CugemderApp.Server.Data;

namespace CugemderApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _appContext;

        public AspNetUsersController(CugemderMobileAppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/AspNetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsers()
        {
            return await _context.AspNetUsers
                .Include(c => c.GroupNavigation)
                .Include(c => c.PointsNavigation)
                .Include(c => c.GenderNavigation)
                .Include(c => c.RelationshipNavigation)
                .Include(c => c.LocatedCityNavigation)
                .OrderByDescending(c => c.PointsNavigation.TotalPoints)
                .ToListAsync();
        }

        [HttpGet]
        [Route("userFullname/{id}")]
        public async Task<ActionResult<string>> GetUserFullname(string id)
        {
            var user = await _context.AspNetUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var fullname = user.FirstName + " " + user.LastName;

            return fullname;


        }

        [HttpGet]
        [Route("NoNull")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersNoNull()
        {
            return await _context.AspNetUsers
                .Include(c => c.GroupNavigation)
                .Include(c => c.PointsNavigation)
                .Include(c => c.RelationshipNavigation)
                .Include(c => c.LocatedCityNavigation)
                .Include(c => c.GenderNavigation)
                .Where(c => c.Points != null)
                .Where(c => c.Group != null)
                .OrderByDescending(c => c.PointsNavigation.TotalPoints)
                .ToListAsync();
        }

        [HttpGet]
        [Route("noGroup")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersWithoutGroups()
        {
            return await _context.AspNetUsers.Where(c => c.Group == null).Include(c => c.GroupNavigation)
                .ToListAsync();
        }

        [HttpGet]
        [Route("NoRole")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersWithoutRoles()
        {
            return await _context.AspNetUsers.Include(c => c.GroupNavigation).Where(x => x.AspNetUserRoles.Count() == 0).ToListAsync();
        }

        // GET: api/AspNetUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUsers>> GetAspNetUsers(string id)
        {
            var aspNetUsers = await _context.AspNetUsers
                .Include(c => c.PositionNavigation)
                .Include(c => c.PointsNavigation)
                .Include(c => c.JobTitleNavigation)
                .Include(c => c.GroupNavigation)
                .Include(c => c.JobTitleNavigation)
                .Include(c => c.RelationshipNavigation)
                .Include(c => c.LocatedCityNavigation)
                .Include(c => c.GenderNavigation)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }

        [HttpGet]
        [Route("getId/{Id}")]
        public async Task<ActionResult<AspNetUsers>> GetAspNetUserID(string Id)
        {
            var aspNetUsers = await _context.AspNetUsers
                 .Include(c => c.PositionNavigation)
                 .Include(c => c.PointsNavigation)
                 .Include(c => c.JobTitleNavigation)
                 .Include(c => c.GroupNavigation)
                 .Include(c => c.JobTitleNavigation)
                 .Include(c => c.RelationshipNavigation)
                 .Include(c => c.LocatedCityNavigation)
                 .Include(c => c.GenderNavigation)
                 .FirstOrDefaultAsync(c => c.Id == Id);

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }

        [HttpGet]
        [Route("username/{email}")]
        public async Task<ActionResult<AspNetUsers>> GetUsername(string email)
        {
            var aspNetUsers = await _context.AspNetUsers
                 .Include(c => c.PositionNavigation)
                 .Include(c => c.PointsNavigation)
                 .Include(c => c.JobTitleNavigation)
                 .Include(c => c.GroupNavigation)
                 .Include(c => c.JobTitleNavigation)
                 .Include(c => c.RelationshipNavigation)
                 .Include(c => c.LocatedCityNavigation)
                 .Include(c => c.GenderNavigation)
                 .FirstOrDefaultAsync(c => c.Email == email);

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }


        [HttpGet]
        [Route("group/{id}")]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetUsersInGroup(int id)
        {
            var aspNetUsers = await _context.AspNetUsers.Where(c => c.Group == id).ToListAsync();
            var test2 = new List<AspNetUsers>();

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }

        // PUT: api/AspNetUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUsers(string id, AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(id))
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

        [HttpPut]
        [Route("photoUrl/{id}")]
        public async Task<IActionResult> PutAspNetUsers(string id, [FromBody] string url)
        {
            var aspNetUsers = await _context.AspNetUsers.Where(c => c.Email == id).FirstOrDefaultAsync();


            if (aspNetUsers == null)
            {
                return NotFound();
            }

            try
            {
                aspNetUsers.PhotoUrl = url;
                _context.Entry(aspNetUsers).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // PUT: api/AspNetUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("newGroup/{id}")]
        public async Task<IActionResult> PutAspNetUsersWithRole(string id, AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(id))
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



        // POST: api/AspNetUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AspNetUsers>> PostAspNetUsers(AspNetUsers aspNetUsers)
        {
            _context.AspNetUsers.Add(aspNetUsers);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUsersExists(aspNetUsers.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUsers", new { id = aspNetUsers.Id }, aspNetUsers);
        }

        // DELETE: api/AspNetUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUsers>> DeleteAspNetUsers(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();

            return aspNetUsers;
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
