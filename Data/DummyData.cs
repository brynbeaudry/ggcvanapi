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
                db.Games.Add(new Game
                {
                    Title = "Marvel vs Capcom",
                    GameDescription = "Marvel vs. Capcom is a series of crossover fighting games developed and published by Capcom, featuring characters from their own video game franchises and comic book series published by Marvel Comics.",
                    NumberOfPlayers = 4,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/6/62/MvCI_Cover.png",
                    Url = "https://en.wikipedia.org/wiki/Marvel_vs._Capcom:_Infinite"
                });
                db.Games.Add(new Game
                {
                    Title = "Defense of the Ancients (DotA)",
                    GameDescription = "Defense of the Ancients (DotA) is a multiplayer online battle arena mod for the video game Warcraft III: Reign of Chaos and its expansion, Warcraft III: The Frozen Throne. ",
                    NumberOfPlayers = 16,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/42/Dota75-loading-screen.png",
                    Url = "https://en.wikipedia.org/wiki/Defense_of_the_Ancients"
                });
                db.Games.Add(new Game
                {
                    Title = "League of Legends",
                    GameDescription = "League of Legends is a multiplayer online battle arena video game developed and published by Riot Games for Microsoft Windows and macOS.",
                    NumberOfPlayers = 16,
                    ImageUrl = "https://i.ytimg.com/vi/L7L7M_PPzJs/hqdefault.jpg",
                    Url = "https://en.wikipedia.org/wiki/League_of_Legends"
                });
                db.Games.Add(new Game
                {
                    Title = "Super Street Fighter II Turbo",
                    GameDescription = "Super Street Fighter II Turbo[a] is a competitive fighting game released for the arcades by Capcom in 1994. It is the fifth installment in the Street Fighter II sub-series of Street Fighter games, following Super Street Fighter II: The New Challengers.",
                    NumberOfPlayers = 2,
                    ImageUrl = "http://backinmyplay.com/wp-content/uploads/2015/12/cover_large.jpg",
                    Url = "https://en.wikipedia.org/wiki/Super_Street_Fighter_II_Turbo"
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
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Super Smash Brothers"),
                    Title = "Crazy Super Smash Brothers Tournament at BCIT"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing MVC @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Marvel vs Capcom"),
                    Title = "Serious Marvel vs Capcom Tournament at BCIT"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing Defense of the Ancients (DotA) @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Defense of the Ancients (DotA)"),
                    Title = "Eighth Defense of the Ancients (DotA) Tournament at BCIT"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing Super Street Fighter II Turbo @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Super Street Fighter II Turbo"),
                    Title = "Second Super Street Fighter II Turbo Tournament at BCIT"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing MVC @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Marvel vs Capcom"),
                    Title = "First Marvel vs Capcom Tournament at BCIT"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing League of Legends @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "League of Legends"),
                    Title = "Super League of Legends Tournament at BCIT"
                });
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
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Super Smash Brothers"),
                    Title = "Say what?! Super Smash Brothers Tournament at BCIT!?"
                });
                db.Events.Add(new Event
                {
                    Creator = db.Users.FirstOrDefault(u=> u.Email == "test@user.com"),
                    Latitude = 49.248499,
                    Longitude = -123.001375,
                    LocationDescription = "BCIT",
                    Host = "BCIT SA",
                    StartTime = DateTime.Now.Add(new TimeSpan(24,0,0)),
                    EndTime = DateTime.Now.Add(new TimeSpan(74, 0, 0)),
                    EventDescription = "Playing Super Street Fighter II Turbo @ Bcit",
                    Game = db.Games.FirstOrDefault(g=> g.Title == "Super Street Fighter II Turbo"),
                    Title = "Super Street Fighter II Turbo Tournament at BCIT"
                });
                


            }
            db.SaveChanges();

            if (!db.EventGuests.Any())
            {
                foreach (var item in db.Events.ToArray())
                {
                    db.EventGuests.Add(new EventGuest
                    {
                        Guest = db.Users.FirstOrDefault(u => u.Email == "test@user.com"),
                        Event = item
                    });
                }
            }
            db.SaveChanges();
        }
    }
}

