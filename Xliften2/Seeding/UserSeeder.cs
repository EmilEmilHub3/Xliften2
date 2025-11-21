using System.Threading.Tasks;
using MongoDB.Driver;
using Xliften2.Models;
using Xliften2.Data;

namespace Xliften2.Seeding
{
    /// <summary>
    /// Helper to ensure a default admin user exists in the Users collection.
    /// Intended for development/testing only.
    /// </summary>
    public static class UserSeeder
    {
        /// <summary>
        /// Inserts a default admin user when the Users collection is empty.
        /// Passwords in this sample are plaintext for simplicity — do not use in production.
        /// </summary>
        /// <param name="context">The <see cref="MongoContext"/> used to access the Users collection.</param>
        public static async Task SeedAdminAsync(MongoContext context)
        {
            // Check for any existing user document.
            var user = await context.Users.Find(_ => true).FirstOrDefaultAsync();

            if (user == null)
            {
                // Insert a simple default admin account.
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
