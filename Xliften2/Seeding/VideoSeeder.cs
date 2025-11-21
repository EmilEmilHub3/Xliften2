using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Xliften2.Data;

namespace Xliften2.Seeding
{
    /// <summary>
    /// One-time helper that seeds sample video files into MongoDB GridFS.
    /// Reads files from a 'Videos' folder in the application's output directory.
    /// </summary>
    public static class VideoSeeder
    {
        /// <summary>
        /// Ensures a set of sample videos exist in the provided <see cref="MongoContext"/>'s GridFS bucket.
        /// Skips files that already exist.
        /// </summary>
        /// <param name="context">The <see cref="MongoContext"/> providing access to the GridFS bucket.</param> 
        public static async Task SeedAsync(MongoContext context)
        {
            // Use the shared GridFS bucket from the provided MongoContext.
            IGridFSBucket bucket = context.VideosBucket;

            // Sample movie filenames. Filenames must match files placed in the project's Videos folder.
            var movieNames = new []
            {
                "LifeIsRoblox.mp4",
                "DogVideo.mp4",
               
            };

            // currentDirectory typically points to bin/Debug/net8.0 when running locally.
            string currentDirectory = Directory.GetCurrentDirectory();

            // Define the local folder path where sample video files are expected.
            string videosFolder = Path.Combine(currentDirectory, "Videos");
            Console.WriteLine("currentDirectory: " + currentDirectory);
            Console.WriteLine("videosFolder: " + videosFolder);

            foreach (var movieName in movieNames)
            {
                // Build a filter to search for an existing GridFS file with the same filename.
                var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, movieName);

                // Execute the query and check if the file already exists in GridFS.
                using (var cursor = await bucket.FindAsync(filter))
                {
                    var existing = (await cursor.ToListAsync()).FirstOrDefault();
                    if (existing != null)
                    {
                        // If the file is already present in GridFS, skip uploading.
                        Console.WriteLine($"[Seed] Skipper '{movieName}' – findes allerede i GridFS med id {existing.Id}");
                        continue;
                    }
                }

                // Full local path to the video file: bin/.../Videos/videoX.mp4
                string fullPath = Path.Combine(videosFolder, movieName);

                // If the local file is missing, inform the user and skip this entry.
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"[Seed] Filen '{fullPath}' findes ikke – tjek at den ligger i projektets 'Videos'-mappe og har 'Copy to Output Directory' slået til.");
                    continue;
                }

                // Read the file bytes and upload them to the GridFS bucket.
                Console.WriteLine($"[Seed] Indlæser og uploader '{fullPath}'...");

                byte[] bytes = await File.ReadAllBytesAsync(fullPath);
                // UploadFromBytesAsync stores the file in GridFS under the provided filename.
                await bucket.UploadFromBytesAsync(movieName, bytes);

                // Confirm completion of the upload for the current file.
                Console.WriteLine($"[Seed] Upload færdig for '{movieName}'.");
            }
        }
    }
}
