using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HRV.Data.Db;

namespace HRV.Data
{
    public class SeedDb
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<UsersDatabaseContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "test@gmail.com",
                    UserName = "test",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "TestKeith",
                    LastName = "TestEvans"
                };

                userManager.CreateAsync(user, "Test@123");
            }
        }

        public static void SeedRange(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<HRVDb>();
            context.Database.EnsureCreated();

            if(!context.Ranges.Any())
            {
                var low = new Db.Range() { Name = "Low", Low = 1, High = 34 };
                var moderate = new Db.Range() { Name = "Moderate", Low = 35, High = 69 };
                var high = new Db.Range() { Name = "High", Low = 70, High = 89 };
                var veryHigh = new Db.Range() { Name = "Very High", Low = 90, High = 100 };

                context.Ranges.Add(low);
                context.Ranges.Add(moderate);
                context.Ranges.Add(high);
                context.Ranges.Add(veryHigh); 
            }

            context.SaveChanges(); 
        }
    }
}
