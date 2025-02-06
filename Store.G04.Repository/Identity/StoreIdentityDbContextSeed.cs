using Microsoft.AspNetCore.Identity;
using Store.G04.core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "amrabbas@gmail.com",
                    DisplayName = "Amr Abbas",
                    UserName = "Amr_Abbas",
                    PhoneNumber = "01111122222",
                    Address = new Address()
                    {
                        FName = "Amr",
                        LName = "Abbas",
                        City = "Nasr City",
                        Country = "Egypt",
                        Street = "Makram"
                    }
                };

                var x = await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        }
    }
}
