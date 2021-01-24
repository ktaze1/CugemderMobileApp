using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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


        [IgnoreDataMember]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
