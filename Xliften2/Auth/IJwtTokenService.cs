namespace Xliften2.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, string role);
    }
}
