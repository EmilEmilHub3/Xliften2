using System.Threading.Tasks;
using MongoDB.Driver;
using Xliften2.Models;
using Xliften2.Data;

namespace Xliften2.Seeding
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(MongoContext context)
        {
            var user = await context.Users.Find(_ => true).FirstOrDefaultAsync();

            if (user == null)
            {
                await context.Users.InsertOneAsync(new User
                {
                    Username = "admin",
                    Password = "admin",
                    Role = "Admin"
                });
            }
        }
    }
}
