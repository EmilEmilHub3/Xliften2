using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xliften2.Repositories;

namespace Xliften2.Endpoints
{
    public static class VideoEndpoints
    {
        public static IEndpointRouteBuilder MapVideoEndpoints(this IEndpointRouteBuilder app)
        {
            // ÅBENT streaming-endpoint (skal kunne kaldes direkte fra <video>-taget)
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
            .WithName("StreamVideo")
            .WithSummary("Streamer en video direkte fra MongoDB GridFS.")
            .WithDescription("Brug ObjectId fra fs.files collection som id for at streame videoen.");

            // BESKYTTET liste over videoer (kræver JWT-token)
            app.MapGet("/videos", async (IGridFsVideoRepository repo) =>
            {
                var videos = await repo.GetAllVideosAsync();
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
