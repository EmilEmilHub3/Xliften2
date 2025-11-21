using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using Xliften2.Auth;
using Xliften2.Data;
using Xliften2.Models;
using Xliften2.Repositories;

namespace Xliften2.Endpoints
{
    /// <summary>
    /// Extension methods to register authentication-related HTTP endpoints.
    /// </summary>
    public static class AuthEndpoints
    {
        /// <summary>
        /// Maps authentication endpoints (login).
        /// </summary>
        /// <param name="app">The endpoint route builder to extend.</param>
        /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
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
