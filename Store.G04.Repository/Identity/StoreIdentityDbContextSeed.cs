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
                    Email = "ahmedaminc41@gmail.com",
                    DisplayName = "Ahmed Amin",
                    UserName = "ahmed_amin",
                    PhoneNumber = "01111122222",
                    Address = new Address()
                    {
                        FName = "Ahmed",
                        LName = "Amin",
                        City = "ElShorouk",
                        Country = "Egypt",
                        Street = "ElShabab"
                    }
                };

                var x = await _userManager.CreateAsync(user, "P@ssW0rd");
                Console.WriteLine($"Flag = {x}");
            }
        }
    }
}
