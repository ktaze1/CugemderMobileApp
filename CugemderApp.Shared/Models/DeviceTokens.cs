using System;
using System.Collections.Generic;

namespace CugemderApp.Shared.Models
{
    public partial class DeviceTokens
    {
        public int Id { get; set; }
        public string DeviceToken { get; set; }
        public string UserEmail { get; set; }
    }
}
