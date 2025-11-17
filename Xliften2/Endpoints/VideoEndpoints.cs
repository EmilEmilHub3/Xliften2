using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xliften2.Repositories;

namespace Xliften2.Endpoints
{
    /// <summary>
    /// Endpoint-layer for video-relaterede endpoints i Xliften (Minimal API).
    /// </summary>
    public static class VideoEndpoints
    {
        /// <summary>
        /// Registrerer video-endpoints på app'en.
        /// </summary>
        public static IEndpointRouteBuilder MapVideoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/video/{id}", async (string id, IGridFsVideoRepository videoRepository) =>
            {
                try
                {
                    var (stream, contentType) = await videoRepository.GetVideoByIdAsync(id);
                    return Results.File(stream, contentType);
                }
                catch (FileNotFoundException)
                {
                    return Results.NotFound($"No video found with id {id}");
                }
            })
            .RequireAuthorization()
            .WithName("StreamVideo")
            .WithSummary("Streamer en video direkte fra MongoDB GridFS.")
            .WithDescription("Brug ObjectId fra fs.files collection som id for at streame videoen.");

            app.MapGet("/videos", async (IGridFsVideoRepository videoRepository) =>
            {
                var videos = await videoRepository.GetAllVideosAsync();
                return Results.Ok(videos);
            })
            .RequireAuthorization()
            .WithName("VideoInfo")
            .WithSummary("Henter metadata for alle videoer fra MongoDB GridFS.")
            .WithDescription("Returnerer liste af videoer med id og title (filnavn), så frontend kan vælge film.");

            return app;
        }
    }
}

