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
        public int TotalPoints { get; set; }

        public int point1 { get; set; }
        public int point2 { get; set; }
        public int point3 { get; set; }
        public int point4 { get; set; }
        public int point5 { get; set; }
        public int point6 { get; set; }
        public int point7 { get; set; }
        public int point8 { get; set; }
        public int point9 { get; set; }
        public int point10 { get; set; }

        public virtual Meetings Meeting { get; set; }
        public virtual AspNetUsers ReceiverUser { get; set; }
    }
}
