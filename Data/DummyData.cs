using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ggcvan.Models;
using ggcvan.Data;

namespace ggcvan.Data
{
    public class DummyData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            if (!db.Users.Any())
            {
                db.Users.Add(new ApplicationUser
                {
                    Email = "test@user.com",
                    Bio = "Wanna play video games dawg",
                    FirstName = "Test",
                    LastName = "User",
                    
                });
            }
            db.SaveChanges();

            if (!db.Games.Any())
            {
                db.Games.Add(new Game
                {
                    Title = "Super Smash Brothers",
                    GameDescription = "The N64 Version of Super Smash Brothers",
                    NumberOfPlayers = 4,
                    ImageUrl = "http://www.yoshismoon.com/soundtracks/soundtrack-smash64.jpg",
                    Url = "https://www.ssbwiki.com/Super_Smash_Bros."
                });
            }
            db.SaveChanges();


            if (!db.Events.Any())
            {
                //seed, call this function from ADBC
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing Smash @ Bcit",
                    Game = db.Games.First(),
                    Title = "SMASH"
                });


            }
            db.SaveChanges();

            if (!db.EventGuests.Any())
            {
                db.EventGuests.Add(new EventGuest
                {
                   Guest = db.Users.FirstOrDefault(u => u.Email == "test@user.com"),
                   Event = db.Events.FirstOrDefault(e=> e.Creator.Email == "test@user.com")
                });
            }
            db.SaveChanges();
        }
    }
}

