using System.Threading.Tasks;
using MongoDB.Driver;
using Xliften2.Data;
using Xliften2.Models;

namespace Xliften2.Repositories
{
    /// <summary>
    /// Repository responsible for authentication-related data access.
    /// Uses <see cref="MongoContext"/> to query the Users collection.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly MongoContext _context;

        /// <summary>
        /// Creates a new instance using the provided MongoDB context.
        /// </summary>
        /// <param name="context">The <see cref="MongoContext"/> for database access.</param>
        public AuthRepository(MongoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Attempts to find a user matching the supplied username and password.
        /// This sample compares plaintext passwords for simplicity — replace with secure hashing in production.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <param name="password">The password to match.</param>
        /// <returns>The matching <see cref="User"/> when found; otherwise null.</returns>
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .Find(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
        }
    }
}
