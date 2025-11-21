using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Xliften2.Models;

namespace Xliften2.Data
{
    /// <summary>
    /// Encapsulates MongoDB connection, database and GridFS bucket configuration.
    /// Reused across services and seeders to share the same client and bucket.
    /// </summary>
    public class MongoContext
    {
        /// <summary>
        /// MongoDB client instance.
        /// </summary>
        public IMongoClient Client { get; }

        /// <summary>
        /// MongoDB database instance.
        /// </summary>
        public IMongoDatabase Database { get; }

        /// <summary>
        /// GridFS bucket configured for storing videos.
        /// </summary>
        public IGridFSBucket VideosBucket { get; }

        /// <summary>
        /// Users collection accessor.
        /// </summary>
        public IMongoCollection<User> Users
            => Database.GetCollection<User>("users");

        /// <summary>
        /// Constructs the <see cref="MongoContext"/> using configuration values.
        /// Expects configuration keys: "MongoSettings:ConnectionString" and "MongoSettings:DatabaseName".
        /// </summary>
        /// <param name="configuration">Application configuration providing MongoDB settings.</param>
        public MongoContext(IConfiguration configuration)
        {
            // Read connection string and database name from configuration.
            Client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            Database = Client.GetDatabase(configuration["MongoSettings:DatabaseName"]);

            // Configure a GridFS bucket named "videos".
            VideosBucket = new GridFSBucket(Database, new GridFSBucketOptions
            {
                BucketName = "videos"
            });
        }
    }
}
