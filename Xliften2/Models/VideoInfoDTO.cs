namespace Xliften2.Models
{
    /// <summary>
    /// Simple DTO for returning video metadata from the API.
    /// </summary>
    public class VideoInfoDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
