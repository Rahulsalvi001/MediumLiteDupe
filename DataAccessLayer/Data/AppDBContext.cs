using EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Data
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserImage> UserImage { get; set; }
        public DbSet<Story> Story { get; set; }
        public DbSet<StoryData> StoryData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var scenarioBuilder = modelBuilder.Entity<AppUser>();
            scenarioBuilder.Property(p => p.UserProfileId).UseIdentityColumn();
            scenarioBuilder.Property(p => p.UserProfileId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
