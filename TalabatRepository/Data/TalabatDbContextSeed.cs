using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace Talabat.Repository.Data
{
    public static class TalabatDbContextSeed
    {
        public static async Task SeedAsync(TalabatDbContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../TalabatRepository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(brand);
                    }
                    await dbContext.SaveChangesAsync();
                } 
            }

            if (!dbContext.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../TalabatRepository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types?.Count > 0)
                {
                    foreach (var type in types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../TalabatRepository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }

            }
        }

        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed Wael",
                    Email = "AhmedWael@gmail.com",
                    UserName = "ahmedwael",
                    PhoneNumber = "0123456789"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
