using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CugemderApp.Shared.Models
{
    public partial class Positions
    {
        public Positions()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string Position { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
