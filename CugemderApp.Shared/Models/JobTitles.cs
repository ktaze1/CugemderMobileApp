using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CugemderApp.Shared.Models
{
    public partial class JobTitles
    {
        public JobTitles()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string TitleName { get; set; }


        [IgnoreDataMember]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
