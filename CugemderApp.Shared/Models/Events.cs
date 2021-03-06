﻿using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Events
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public int? RelatedGroup { get; set; }

        public virtual Groups RelatedGroupNavigation { get; set; }
    }
}
