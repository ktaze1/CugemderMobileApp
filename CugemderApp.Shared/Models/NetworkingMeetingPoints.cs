using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class NetworkingMeetingPoints
    {
        public int Id { get; set; }
        public string ReceiverUserId { get; set; }
        public int TotalPoints { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AddedBy { get; set; }

        public virtual AspNetUsers ReceiverUser { get; set; }
    }
}
