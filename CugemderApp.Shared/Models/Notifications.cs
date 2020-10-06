using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Notifications
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }
    }
}
