using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Tesfay",
                    Email = "tesfayb@test.com",
                    UserName = "tesfayb@test.com",
                    Address = new Address
                    {
                        FristName = "Tesfay",
                        LastName = "Bsrat",
                        Street = "01 biro",
                        City = "Tigray",
                        State = "Mekelle",
                        ZipCode = "212645"
                    }
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}