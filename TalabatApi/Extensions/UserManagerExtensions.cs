using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalabatCore.Entities;


namespace Talabat.APIS.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindUserWithAddressAsync (this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue (ClaimTypes.Email);
            var User = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            return User;
        }
    }
}
