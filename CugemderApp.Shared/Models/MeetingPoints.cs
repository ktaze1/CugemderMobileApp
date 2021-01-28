using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class MeetingPoints
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string ReceiverUserId { get; set; }
        public string SenderUserId { get; set; }
        public int Point1 { get; set; }
        public int Point2 { get; set; }
        public int Point3 { get; set; }
        public int Point4 { get; set; }
        public int Point5 { get; set; }
        public int Point6 { get; set; }
        public int Point7 { get; set; }
        public int Point8 { get; set; }
        public int Point9 { get; set; }
        public int Point10 { get; set; }
        public int TotalPoints { get; set; }

        public virtual Meetings Meeting { get; set; }
        public virtual AspNetUsers ReceiverUser { get; set; }
    }
}
