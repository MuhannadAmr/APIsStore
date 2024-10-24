using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.DEMO.APIs.Errors;
using Store.DEMO.Core.Entites.Identity;
using System.Security.Claims;

namespace Store.DEMO.APIs.Extenstions
{
    public static class UserManagerExtenstions
    {
        public async static Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return null;
            var user = await userManager.Users.Include(A => A.Address).FirstOrDefaultAsync(U => U.Email == userEmail);
            if (user is null) return null;

            return user;
        }
    }
}
