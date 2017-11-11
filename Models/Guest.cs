using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ggcvan.Models
{
    public class Guest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guest(ApplicationUser user)
        {
            this.Id = user.Id;
            this.FirstName = user.FullName;
            this.LastName = user.LastName;
        }

        //public String Bio { get; set; }

        //public List<Event> CreatedEvents { get; set; }

        //public List<EventGuest> JoinedEvents { get; set; }
    }
}
