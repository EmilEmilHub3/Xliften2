using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Endpoints;
using Xliften2.Repositories;
using Xliften2.Seeding;

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

            // Mongo
            builder.Services.AddSingleton<MongoContext>();
            builder.Services.AddSingleton<IGridFsVideoRepository, GridFsVideoRepository>();

            // Auth repository (for /login)
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // JWT settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // 🚨 CORS – DEV MODE: tillad ALT (så vi er sikre det ikke er CORS der blokerer)
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

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

            // 🔹 Brug CORS FØR auth/authorization
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            // Seed admin-bruger og videoer
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
                RequestPath = "/StaticFiles",
                EnableDefaultFiles = true
            });

            // Endpoints
            app.MapAuthEndpoints();   // /login
            app.MapVideoEndpoints();  // /videos + /video/{id}

            app.MapGet("/", () => "Xliften API running");

            app.Run();
        }
    }
}
