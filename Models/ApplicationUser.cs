using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ggcvan.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser 
    {
        
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName { get; set; }

        public String ProviderName { get; set; }

        public String ProviderId { get; set; }  
        public String PictureUrl { get; set; }

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

        public override string UserName { get; set; }
        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        [JsonIgnore]
        [ScaffoldColumn(false)]
        [NotMapped]
        public bool SerializeJE { get; set; }
        [JsonIgnore]
        [ScaffoldColumn(false)]
        [NotMapped]
        public bool SerializeCE { get; set; }
        [JsonIgnore]
        [ScaffoldColumn(false)]
        [NotMapped]
        public bool SerializeBio { get; set; }

        //[JsonIgnore]
        public String Bio {get; set;}

        public List<Event> CreatedEvents { get; set; }

        public List<EventGuest> JoinedEvents { get; set; }

        public bool ShouldSerializeJoinedEvents()
        {
            return SerializeJE;        
        }

        public bool ShouldSerializeCreatedEvents()
        {
            return SerializeCE;
        }

        public bool ShouldSerializeBio()
        {
            return SerializeBio;
        }

    }
}
