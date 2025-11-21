using System.Threading.Tasks;
using Xliften2.Models;

namespace Xliften2.Repositories
{
    /// <summary>
    /// Repository contract for authentication operations against the user store.
    /// Implementations should validate provided credentials and return the matching <see cref="User"/> or null.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Attempts to authenticate a user by username and password.
        /// </summary>
        /// <param name="username">The username to authenticate.</param>
        /// <param name="password">The password to authenticate. Implementations should compare hashed values where appropriate.</param>
        /// <returns>
        /// The matching <see cref="User"/> when authentication succeeds; otherwise null.
        /// </returns>
        Task<User?> AuthenticateAsync(string username, string password);
    }

}
