using System.Threading.Tasks;
using MongoDB.Driver;
using Xliften2.Data;
using Xliften2.Models;

namespace Xliften2.Repositories
{
    /// <summary>
    /// Repository responsible for authentication-related data access.
    /// Uses MongoContext to query the Users collection.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly MongoContext _context;

        public AuthRepository(MongoContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users
                .Find(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
        }
    }
}
