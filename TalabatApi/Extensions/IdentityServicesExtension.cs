using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Talabat.Repository.Data;
using TalabatCore.Entities;
using TalabatCore.Services;
using TalabatServices;


namespace Talabat.APIS.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddScoped<ITokenService, TokenService>();

            Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<TalabatDbContext>();

            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

            Services.AddAuthentication();

            return Services;
        }
    }
}
