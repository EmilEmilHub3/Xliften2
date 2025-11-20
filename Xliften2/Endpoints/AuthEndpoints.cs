using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Models;
using Xliften2.Repositories;

namespace Xliften2.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (LoginRequestDTO req, IAuthRepository authRepo, IJwtTokenService tokenService) =>
            {
                var user = await authRepo.AuthenticateAsync(req.Username, req.Password);

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
