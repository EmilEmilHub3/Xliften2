using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xliften2.Repositories;

namespace Xliften2.Endpoints
{
    /// <summary>
    /// Extension methods for mapping video-related HTTP endpoints.
    /// </summary>
    public static class VideoEndpoints
    {
        /// <summary>
        /// Registers endpoints for streaming a single video and retrieving video metadata list.
        /// </summary>
        /// <param name="app">The endpoint route builder to extend.</param>
        /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
        public static IEndpointRouteBuilder MapVideoEndpoints(this IEndpointRouteBuilder app)
        {
            // Streaming endpoint (can be called directly from a <video> element).
            app.MapGet("/video/{id}", async (string id, IGridFsVideoRepository repo) =>
            {
                try
                {
                    var (stream, contentType) = await repo.GetVideoByIdAsync(id);
                    return Results.File(stream, contentType);
                }
                catch (FileNotFoundException)
                {
                    return Results.NotFound($"No video found with id {id}");
                }
            })
            .RequireAuthorization()
            .WithName("StreamVideo")
            .WithSummary("Streams a video directly from MongoDB GridFS.")
            .WithDescription("Use the ObjectId from the fs.files collection as the id to stream the video.");

            // Protected list of available videos (requires JWT token).
            app.MapGet("/videos", async (IGridFsVideoRepository repo) =>
            {
                var videos = await repo.GetAllVideosAsync();
                return Results.Ok(videos);
            })
            .RequireAuthorization()
            .WithName("VideoInfo")
            .WithSummary("Retrieves metadata for all videos from MongoDB GridFS.")
            .WithDescription("Returns a list of videos with id and title (filename) so the frontend can choose a movie.");

            return app;
        }
    }
}
