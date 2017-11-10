using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ggcvan.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser 
    {   
        public String FullName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public String Bio {get; set;}

        public List<Event> CreatedEvents { get; set; }

        public List<EventGuest> JoinedEvents { get; set; }

    }
}
