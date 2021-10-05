using System;
using HRV.Data.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HRV.Data
{
    public class UsersDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public UsersDatabaseContext(DbContextOptions<UsersDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var intMaxLength = 100;
            builder.Entity<ApplicationUser>(au =>
            {
                au.Property(r => r.Id).HasMaxLength(intMaxLength);
            });

            builder.Entity<ApplicationRole>(ar =>
            {
                ar.Property(r => r.Id).HasMaxLength(intMaxLength);
                ar.Property(r => r.Name).HasMaxLength(intMaxLength);
                ar.Property(r => r.NormalizedName).HasMaxLength(intMaxLength);
            });

            
            builder.Entity<IdentityUserLogin<string>>(iul => {
                iul.Property(i => i.LoginProvider).HasMaxLength(intMaxLength);
                iul.Property(i => i.ProviderKey).HasMaxLength(intMaxLength);
                iul.HasKey(i => new { i.ProviderKey, i.LoginProvider });
            });

            builder.Entity<IdentityUserToken<string>>(iut =>
            {
                iut.Property(i => i.LoginProvider).HasMaxLength(intMaxLength);
                iut.Property(i => i.Name).HasMaxLength(intMaxLength);
                iut.HasKey(i => new { i.Name, i.UserId, i.LoginProvider });

            });
            
            base.OnModelCreating(builder);
        }
    }
}
