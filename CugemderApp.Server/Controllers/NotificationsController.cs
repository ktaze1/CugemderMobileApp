using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CugemderApp.Shared.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using System.Text;
using System.Globalization;

namespace CugemderApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly CugemderMobileAppDbContext _context;
        private FirebaseApp _firebaseApp = FirebaseApp.DefaultInstance == null ? FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("C:\\Users\\Kaan\\Downloads\\cugemdermobile-firebase-adminsdk-wgo74-9dc80b63a3.json"),
        }) : FirebaseApp.DefaultInstance;
        public NotificationsController(CugemderMobileAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<string> SendNotif()
        {
            // See documentation on defining a message payload.
            //var message = new Message()
            //{
            //    Notification = new Notification()
            //    {
            //        Title = title,
            //        Body = body,
            //    },
                
            //    Topic = topic,
            //};

            //// Send a message to the devices subscribed to the provided topic.
            //string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            //// Response is a message ID string.
            //Console.WriteLine("Successfully sent message: " + response);
            return "";
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notifications>> GetNotifications(int id)
        {
            var notifications = await _context.Notifications.FindAsync(id);

            if (notifications == null)
            {
                return NotFound();
            }

            return notifications;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notifications>>> GetNotifications()
        {
            return await _context.Notifications.Where(c => c.Time.Date == DateTime.Now.Date).ToListAsync();
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotifications(int id, Notifications notifications)
        {
            if (id != notifications.Id)
            {
                return BadRequest();
            }

            _context.Entry(notifications).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationsExists(id))
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

        // POST: api/Notifications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Notifications>> PostNotifications(Notifications notifications)
        {
            _context.Notifications.Add(notifications);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotifications", new { id = notifications.Id }, notifications);
        }

        [HttpPost]
        [Route("sendNotification")]
        public async void SendNotification(NotificationObject notification)
        {

            var normalizedTopic = RemoveDiacritics(notification.topic);
            
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = notification.title,
                    Body = notification.body,
                },

                Topic = normalizedTopic,
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notifications>> DeleteNotifications(int id)
        {
            var notifications = await _context.Notifications.Where(c => c.GroupId == id).FirstOrDefaultAsync();
            if (notifications == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notifications);
            await _context.SaveChangesAsync();

            return notifications;
        }

        private bool NotificationsExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }

        private static string RemoveDiacritics(string text)
        {
            Encoding srcEncoding = Encoding.UTF8;
            Encoding destEncoding = Encoding.GetEncoding(1252); // Latin alphabet

            text = destEncoding.GetString(Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(text)));

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                if (!CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]).Equals(UnicodeCategory.NonSpacingMark))
                {
                    result.Append(normalizedString[i]);
                }
            }

            return result.ToString();
        }
    }
}
