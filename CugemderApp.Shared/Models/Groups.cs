using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Groups
    {
        public Groups()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Events = new HashSet<Events>();
        }

        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<Events> Events { get; set; }
    }
}
