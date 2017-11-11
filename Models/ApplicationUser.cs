using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ggcvan.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser 
    {
        
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName { get; set; }
        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }
        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }
        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }
        [JsonIgnore]
        public override string PhoneNumber { get; set; }
        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
        [JsonIgnore]
        public override string SecurityStamp { get; set; }
        [JsonIgnore]
        public override string PasswordHash { get; set; }
        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }
        [JsonIgnore]
        public override string NormalizedEmail { get; set; }
        [JsonIgnore]
        public override string Email { get; set; }
        [JsonIgnore]
        public override string NormalizedUserName { get; set; }
        [JsonIgnore]
        public override string UserName { get; set; }
        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        //[JsonIgnore]
        public String Bio {get; set;}

        public List<Event> CreatedEvents { get; set; }

        public List<EventGuest> JoinedEvents { get; set; }

        public bool ShouldSerializeJoinedEvents(bool t)
        {
            return t; //maybe a more meaningful logic
        }
        public bool ShouldSerializeCreatedEvents(bool t)
        {
            return t; //maybe a more meaningful logic
        }
        public bool ShouldSerializeBio(bool t)
        {
            return t; //maybe a more meaningful logic
        }

    }
}
