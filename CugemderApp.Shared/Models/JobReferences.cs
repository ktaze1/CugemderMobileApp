using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class JobReferences
    {
        public int Id { get; set; }
        public string ReferencerId { get; set; }
        public string ExpertName { get; set; }
        public string ExpertContact { get; set; }
        public string ReferencedId { get; set; }
        public bool IsMeetingDone { get; set; }
        public bool? IsProductive { get; set; }
        public DateTime? Date { get; set; }
    }
}
