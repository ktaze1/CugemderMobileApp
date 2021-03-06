﻿using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Meetings
    {
        public Meetings()
        {
            MeetingPoints = new HashSet<MeetingPoints>();
        }

        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public bool IsApproved { get; set; }
        public bool IsResultedbySender { get; set; }
        public bool IsResultedbyReceiver { get; set; }

        public virtual ICollection<MeetingPoints> MeetingPoints { get; set; }
    }
}
