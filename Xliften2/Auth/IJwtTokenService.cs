    namespace Xliften2.Auth
{
    /// <summary>
    /// Service contract for generating JWT tokens used for authentication/authorization.
    /// Implementations should sign and return a compact JWT string containing necessary claims.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a signed JWT for the specified user.
        /// </summary>
        /// <param name="username">The username to include in the token subject/claims.</param>
        /// <param name="role">The role to include as a claim (e.g., "Admin").</param>
        /// <returns>A signed JWT as a compact string.</returns>
        string GenerateToken(string username, string role);
    }
}
