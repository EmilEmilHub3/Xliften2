using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Xliften2.Data;
using Xliften2.Models;
using Xliften2.Repositories;

namespace Xliften2.Repositories
{
    /// <summary>
    /// Repository responsible for reading video files and metadata from MongoDB GridFS.
    /// </summary>
    public class GridFsVideoRepository : IGridFsVideoRepository
    {
        private readonly IGridFSBucket _bucket;

       
        public GridFsVideoRepository(MongoContext context)
        {
            // Use the shared GridFS bucket from the provided context.
            _bucket = context.VideosBucket;
        }

        /// <summary>
        /// Finds and retrieves a video file from GridFS by its ObjectId.
        /// Returns both a readable stream and the MIME content type (from metadata if present).
        /// </summary>
        /// <param name="fileId">String representation of the GridFS ObjectId.</param>
        /// <returns>
        /// A tuple containing:
        /// - Stream Stream: the downloaded file stream (caller is responsible for disposing it).
        /// - string ContentType: the MIME type read from file metadata or a default value.
        /// </returns>
        public async Task<(Stream Stream, string ContentType)> GetVideoByIdAsync(string fileId)
        {
            // Parse the string id into a MongoDB ObjectId.
            var objectId = ObjectId.Parse(fileId);

            // Look up the file info to verify existence and read metadata.
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Id, objectId);
            var fileInfo = await _bucket.Find(filter).FirstOrDefaultAsync();

            if (fileInfo == null)
                throw new FileNotFoundException($"No file with id {fileId}");

            // Download the file contents into a memory stream.
            var memoryStream = new MemoryStream();
            await _bucket.DownloadToStreamAsync(objectId, memoryStream);
            memoryStream.Position = 0;

            // Default content type if none is provided in metadata.
            var contentType = "application/octet-stream";
            if (fileInfo.Metadata != null && fileInfo.Metadata.Contains("contentType"))
                contentType = fileInfo.Metadata["contentType"].AsString;

            return (memoryStream, contentType);
        }

        /// <summary>
        /// Retrieves id and title (filename) for all videos stored in GridFS.
        /// Useful for populating a frontend list of available videos.
        /// </summary>
        /// <returns>A read-only list of <see cref="VideoInfoDTO"/> entries with Id and Title.</returns>
        public async Task<IReadOnlyList<VideoInfoDTO>> GetAllVideosAsync()
        {
            // Query all files in the GridFS bucket.
            var filter = Builders<GridFSFileInfo>.Filter.Empty;
            var cursor = await _bucket.FindAsync(filter);
            var files = await cursor.ToListAsync();

            var result = new List<VideoInfoDTO>();

            // Map GridFS file info to VideoInfoDTO (id and filename).
            foreach (var file in files)
            {
                result.Add(new VideoInfoDTO
                {
                    Id = file.Id.ToString(),
                    Title = file.Filename
                });
            }

            return result;
        }
    }
}
