﻿using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Genders
    {
        public Genders()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int Id { get; set; }
        public string GenderName { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
