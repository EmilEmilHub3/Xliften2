using Xliften2.Models;

namespace Xliften2.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }

}
