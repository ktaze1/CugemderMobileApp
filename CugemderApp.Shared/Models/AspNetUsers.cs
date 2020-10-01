using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CugemderApp.Shared.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            MeetingPoints = new HashSet<MeetingPoints>();
            MeetingsReceiver = new HashSet<Meetings>();
            MeetingsSender = new HashSet<Meetings>();
            Notifications1 = new HashSet<Notifications>();
            Points1 = new HashSet<Points>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string CustomClaim { get; set; }
        public string Company { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string FirstName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Group { get; set; }
        public int? JobTitle { get; set; }
        public int? Notifications { get; set; }
        public string PhotoUrl { get; set; }
        public int? Points { get; set; }
        public int? Position { get; set; }
        public string Speciality { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [StringLength(500, ErrorMessage ="Kişi özeti 500 karakterden uzun olamaz!")]
        public string Summary { get; set; }
        public string Website { get; set; }
        public int? Year { get; set; }
        public int? Standings { get; set; }
        public string LastName { get; set; }
        public int? Relationship { get; set; }
        public int? LocatedCity { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual Genders GenderNavigation { get; set; }
        public virtual Groups GroupNavigation { get; set; }
        public virtual JobTitles JobTitleNavigation { get; set; }
        public virtual Cities LocatedCityNavigation { get; set; }
        public virtual Notifications NotificationsNavigation { get; set; }
        public virtual Points PointsNavigation { get; set; }
        public virtual Positions PositionNavigation { get; set; }
        public virtual Relationship RelationshipNavigation { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<MeetingPoints> MeetingPoints { get; set; }
        public virtual ICollection<Meetings> MeetingsReceiver { get; set; }
        public virtual ICollection<Meetings> MeetingsSender { get; set; }
        public virtual ICollection<Notifications> Notifications1 { get; set; }
        public virtual ICollection<Points> Points1 { get; set; }
    }
}
