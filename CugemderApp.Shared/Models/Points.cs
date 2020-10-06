using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Points
    {
        public Points()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string AddedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double TotalPoints { get; set; }
        public string UserId { get; set; }
        public int CategoryMeetingPoint { get; set; }
        public int CategoryNetworkingMeetingPoint { get; set; }
        public int CategoryNetworkingActivityPoint { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
