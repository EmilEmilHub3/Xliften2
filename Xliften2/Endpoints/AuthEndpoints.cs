using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using Xliften2.Auth;
using Xliften2.Models;
using Xliften2.Data;

namespace Xliften2.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (LoginRequest req, MongoContext db, IJwtTokenService tokenService) =>
            {
                var user = await db.Users
                    .Find(u => u.Username == req.Username && u.Password == req.Password)
                    .FirstOrDefaultAsync();

                if (user == null)
                    return Results.Unauthorized();

                var token = tokenService.GenerateToken(user.Username, user.Role);
                return Results.Ok(new { token });
            })
            .AllowAnonymous();

            return app;
        }
    }
}
