using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Endpoints;
using Xliften2.Repositories;
using Xliften2.Seeding;

namespace Xliften
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Mongo
            builder.Services.AddSingleton<MongoContext>();
            builder.Services.AddSingleton<IGridFsVideoRepository, GridFsVideoRepository>();

            // JWT settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // Authentication + Authorization
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var cfg = builder.Configuration.GetSection("JwtSettings");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = cfg["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = cfg["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["SigningKey"])),
                        ValidateLifetime = true
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Seeding
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MongoContext>();
                UserSeeder.SeedAdminAsync(db).Wait();
                VideoSeeder.SeedAsync(db).Wait();
            }

            // Static files – peg på mappen "StaticFiles"
            var staticFilesPath = Path.Combine(builder.Environment.ContentRootPath, "StaticFiles");

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath),
                RequestPath = "/StaticFiles",      // URL-prefix
                EnableDefaultFiles = true          // så /StaticFiles loader index.html automatisk
            });

            // Endpoints
            app.MapAuthEndpoints();
            app.MapVideoEndpoints();

            app.MapGet("/", () => "Xliften API running");

            app.Run();
        }
    }
}
