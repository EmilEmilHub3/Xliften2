using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xliften2.Models;

namespace Xliften2.Repositories
{
    /// <summary>
    /// Provides methods to interact with GridFS-stored video data.
    /// Implementations should handle streaming, content types and listing metadata.
    /// </summary>
    public interface IGridFsVideoRepository
    {
        /// <summary>
        /// Retrieves a video file stream and its content type by file identifier.
        /// </summary>
        /// <param name="fileId">The identifier of the file in GridFS.</param>
        /// <returns>
        /// A tuple containing:
        /// - Stream Stream: the video file stream (caller is responsible for disposing).
        /// - string ContentType: the MIME content type for the stream (e.g., "video/mp4").
        /// </returns>
        Task<(Stream Stream, string ContentType)> GetVideoByIdAsync(string fileId);

        /// <summary>
        /// Retrieves metadata for all videos stored in GridFS.
        /// </summary>
        /// <returns>A read-only list of <see cref="VideoInfoDTO"/> objects.</returns>
        Task<IReadOnlyList<VideoInfoDTO>> GetAllVideosAsync();
    }
}