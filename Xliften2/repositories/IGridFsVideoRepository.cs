using Xliften2.Models;

namespace Xliften2.Repositories
{
    public interface IGridFsVideoRepository
    {
        Task<(Stream Stream, string ContentType)> GetVideoByIdAsync(string fileId);
        Task<IReadOnlyList<VideoInfoDTO>> GetAllVideosAsync();
    }
}