using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class MeetingPoints
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string ReceiverUserId { get; set; }
        public int TotalPoints { get; set; }

        public virtual AspNetUsers ReceiverUser { get; set; }
    }
}
