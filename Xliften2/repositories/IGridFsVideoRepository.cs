using Xliften.Models;

namespace Xliften2.repositories
{
    public interface IGridFsVideoRepository
    {
        Task<(Stream Stream, string ContentType)> GetVideoByIdAsync(string fileId);
        Task<IReadOnlyList<VideoInfo>> GetAllVideosAsync();
    }
}