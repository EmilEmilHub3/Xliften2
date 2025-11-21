namespace Xliften2.Models
{
    /// <summary>
    /// Simple DTO for returning video metadata from the API.
    /// </summary>
    public class VideoInfoDTO
    {
        /// <summary>
        /// Identifier for the video (e.g., GridFS file id or database id).
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable title for the video.
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
