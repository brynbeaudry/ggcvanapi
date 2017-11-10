using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ggcvan.Models
{
    //M2m assoc class
    public class EventGuest
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string GuestId { get; set; }
        public ApplicationUser Guest { get; set; }

    }
}
