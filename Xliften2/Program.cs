using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Xliften.Data;
using Xliften.Endpoints;
using Xliften.Seeding;
using Xliften.Services;
using Xliften.Services.ServiceInterfaces;

namespace Xliften
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔹 Swagger + endpoint explorer
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 🔹 Registrér MongoContext som singleton
            builder.Services.AddSingleton<MongoContext>();

            // 🔹 Registrér din video-service
            builder.Services.AddSingleton<IGridFsVideoService, GridFsVideoService>();

            var app = builder.Build();

            // 🔹 Swagger i development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // (valgfrit) HTTPS-redirect hvis du vil
            // app.UseHttpsRedirection();

            // 🔹 Kør seeding én gang ved opstart (genbruger MongoContext)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MongoContext>();
                VideoSeeder
                    .SeedAsync(context)
                    .GetAwaiter()
                    .GetResult();
            }

            //(Valgfrit) Static files som din lærer viser, hvis du har mappen "StaticFiles"
            
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/StaticFiles",
                EnableDefaultFiles = true
            });
            

            // 🔹 Lille test-endpoint til at se om API kører
            app.MapGet("/", () => "Xliften API is running 🚀");

            // 🔹 Dine video-endpoints (de filer du har sendt)
            app.MapVideoEndpoints();

            app.Run();
        }
    }
}
