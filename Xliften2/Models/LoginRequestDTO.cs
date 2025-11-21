namespace Xliften2.Models
{
    /// <summary>
    /// DTO used to receive login requests from clients.
    /// Contains credentials required for authentication.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Username submitted by the client.
        /// Defaults to empty string to avoid nulls.
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        /// Password submitted by the client.
        /// Defaults to empty string.
        /// </summary>
        public string Password { get; set; } = "";
    }
}
