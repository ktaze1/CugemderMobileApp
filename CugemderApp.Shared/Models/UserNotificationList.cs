using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class UserNotificationList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; set; }
    }
}
