using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Cities
    {
        public Cities()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
