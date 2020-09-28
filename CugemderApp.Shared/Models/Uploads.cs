using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class Uploads
    {
        public int Id { get; set; }
        public string UserMail { get; set; }
        public string FileName { get; set; }

        public virtual AspNetUsers UserMailNavigation { get; set; }
    }
}
