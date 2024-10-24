using Microsoft.AspNetCore.Identity;
using Store.DEMO.Core.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        
        public async static Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "mohanedamr2015@gmail.com",
                    DisplayName = "Muhannad Amr",
                    UserName = "MuhannadAmr",
                    PhoneNumber = "01141400599",
                    Address = new Address()
                    {
                        FName = "Muhannad",
                        LName = "Amr",
                        City = "Cairo",
                        Street = "Sheraton",
                        Country = "Egypt"
                    }
                };
                await userManager.CreateAsync(user,"P@ssW0rd");
            }
        }
    }
}
