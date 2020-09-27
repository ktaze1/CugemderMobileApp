using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Relationship
    {
        public Relationship()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string RelationshipStatus { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
