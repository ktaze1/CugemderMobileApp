using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class ForgotPasswordRequests
    {
        public string Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
