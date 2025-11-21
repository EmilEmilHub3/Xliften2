using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Endpoints;
using Xliften2.Repositories;
using Xliften2.Seeding;

// Author: Emil Riber Rosholm
// Date: 2025-11-21

namespace Xliften2
{
    /// <summary>
    /// Application entry point for the Xliften API.
    /// </summary>
    /// <author>Emil Riber Rosholm</author>
    /// <date>2025-11-21</date>
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

            // Auth repository (for /login)
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

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
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(cfg["SigningKey"])
                        ),
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

            app.UseAuthentication();
            app.UseAuthorization();

            // Seed admin user and sample videos
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MongoContext>();
                UserSeeder.SeedAdminAsync(db).Wait();
                VideoSeeder.SeedAsync(db).Wait();
            }

            // Endpoints
            app.MapAuthEndpoints();   // /login
            app.MapVideoEndpoints();  // /videos + /video/{id}

            app.Run();
        }
    }
}
