using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CugemderApp.Server.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string CustomClaim { get; set; }
        public string Company { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string FirstName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Group { get; set; }
        public int? JobTitle { get; set; }
        public string PhotoUrl { get; set; }
        public int? Points { get; set; }
        public int? Position { get; set; }
        public string Speciality { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Summary { get; set; }
        public string Website { get; set; }
        public int? Year { get; set; }
        public int? Standings { get; set; }
        public string LastName { get; set; }

    }
}
