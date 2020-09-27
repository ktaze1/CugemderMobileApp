using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CugemderApp.Shared.Models
{
    public partial class Events
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime? Date { get; set; }
    }
}
