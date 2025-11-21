namespace Xliften2.Auth
{
    /// <summary>
    /// Configuration POCO for JWT: issuer, audience and signing key.
    /// Bound from configuration (e.g. appsettings.json).
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Token issuer identifier.
        /// </summary>
        public string Issuer { get; set; } = "";

        /// <summary>
        /// Token audience.
        /// </summary>
        public string Audience { get; set; } = "";

        /// <summary>
        /// Symmetric signing key (base text). Keep this secret and sufficiently long.
        /// </summary>
        public string SigningKey { get; set; } = "";
    }
}
