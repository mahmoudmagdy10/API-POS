using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Mahmoud Magdy",
                    Email = "MahmoudMagdy@gmail.com",
                    UserName = "MahmoudMagdy",
                    PhoneNumber = "01209897654"
                };
                await userManager.CreateAsync(user, "Mego@123");
            }
        }
    }
}
