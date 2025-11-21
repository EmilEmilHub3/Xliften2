namespace Xliften2.Models
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user (could be a GUID or database id).
        /// Defaults to empty string to avoid nulls.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Login username for the user.
        /// Defaults to empty string.
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        /// Hashed password for the user.
        /// Store hashed values; avoid storing plaintext in production.
        /// Defaults to empty string.
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// Role assigned to the user (e.g., "Admin", "User").
        /// Default is "Admin".
        /// </summary>
        public string Role { get; set; } = "Admin";
    }
}
