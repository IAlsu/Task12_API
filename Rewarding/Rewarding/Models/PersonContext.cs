using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rewarding.Models
{
    public class PersonContext: IdentityDbContext<ApplicationUser>
    {
        public PersonContext() : base("PersonContext") { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Image> Pictures { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reward>().HasMany(c => c.Persons)
                .WithMany(s => s.Rewards)
                .Map(t => t.MapLeftKey("RewardId")
                .MapRightKey("PersonId")
                .ToTable("PersonRewards"));

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
        public static PersonContext Create()
        {
            return new PersonContext();
        }

        public System.Data.Entity.DbSet<Rewarding.Models.ApplicationRole> IdentityRoles { get; set; }
    }
}

