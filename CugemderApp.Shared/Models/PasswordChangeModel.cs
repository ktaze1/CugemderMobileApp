using System;
using System.Collections.Generic;
using System.Text;

namespace CugemderApp.Shared.Models
{
    public class PasswordChangeModel
    {
        public string id { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }
    }
}
