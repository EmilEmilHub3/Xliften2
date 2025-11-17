using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Endpoints;
using Xliften2.Seeding;
using Xliften2.Repositories; // dit repository namespace

namespace Xliften2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // MongoContext + Repository
            builder.Services.AddSingleton<MongoContext>();
            builder.Services.AddSingleton<IGridFsVideoRepository, GridFsVideoRepository>();

            // JWT Settings fra appsettings.json
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // Token service
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // Authentication
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSection = builder.Configuration.GetSection("JwtSettings");
                    var issuer = jwtSection["Issuer"];
                    var audience = jwtSection["Audience"];
                    var signingKey = jwtSection["SigningKey"];

                    // Validate configuration early to avoid cryptic exceptions (e.g. Encoding.GetBytes(null))
                    if (string.IsNullOrWhiteSpace(signingKey))
                    {
                        throw new InvalidOperationException("JwtSettings:SigningKey is missing or empty in appsettings.json (or env configuration).");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                        ValidateLifetime = true
                    };
                });

            // Authorization
            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ⛔ VIGTIGT: AUTH MIDDLEWARE
            app.UseAuthentication();
            app.UseAuthorization();

            // Seeding
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MongoContext>();
                VideoSeeder.SeedAsync(context).GetAwaiter().GetResult();
            }

            // Static files
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles",
                EnableDefaultFiles = true
            });

            // Test endpoint
            app.MapGet("/", () => "Xliften API is running 🚀");

            // Auth endpoints (login)
            app.MapAuthEndpoints();

            // Video endpoints (RequireAuthorization inde i endpoints-filen)
            app.MapVideoEndpoints();

            app.Run();
        }
    }
}
