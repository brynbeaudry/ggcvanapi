using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ggcvan.Models
{
    public class EventCreator : ApplicationUser
    {
        public EventCreator() : base()
        {

        }

        public EventCreator(ApplicationUser user)
        {
            this.Id = user.Id;
            this.FirstName = user.FullName;
            this.LastName = user.LastName;
        }
        [JsonIgnore]
        public new String Bio { get; set; }
        [JsonIgnore]
        public new List<Event> CreatedEvents { get; set; }
        [JsonIgnore]
        public new List<EventGuest> JoinedEvents { get; set; }
    }
}
