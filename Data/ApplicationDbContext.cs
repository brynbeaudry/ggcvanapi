using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ggcvan.Models;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace ggcvan.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DBSet<OpenIdDictApplications> OpenIdDictApplications { get;set;}
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Game> Games { get; set; }
        //  public DbSet<GameRank> GameRanks { get; set; }
        //public DbSet<Leaderboard> Leaderboards { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //Relations

            builder.Entity<ApplicationUser>()
                .HasIndex("ProviderId")
                .IsUnique()
                .HasName("ProviderIdIndex");
            
            builder.Entity<ApplicationUser>()
                .HasIndex("UserName")
                .HasName("NonNormalizedUserNameIndex");

            builder.Entity<ApplicationUser>()
                .HasIndex("ProviderName")
                .HasName("ProviderNameIndex");
            //can make forieg key later

            builder.Entity<Event>()
            .Property<int>("EventGame");
            builder.Entity<Event>()
                .HasOne(e => e.Game);


            builder.Entity<Event>()
            .Property<string>("EventCreator");
            builder.Entity<Event>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.CreatedEvents)
                .HasForeignKey("EventCreator");

            builder.Entity<EventGuest>()
            .HasKey(eg => new { eg.GuestId, eg.EventId });
            builder.Entity<EventGuest>()
                .HasOne(eg => eg.Event)
                .WithMany(e => e.EventGuests)
                .HasForeignKey(eg => eg.EventId);
            builder.Entity<EventGuest>()
                .HasOne(eg => eg.Guest)
                .WithMany(a => a.JoinedEvents)
                .HasForeignKey(eg => eg.GuestId);


        }

    }
}
