using Xliften2.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Xliften2.Models;

namespace Xliften2.Endpoints
{
    
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            // POST /login  -> returns JWT if username/password are valid
            app.MapPost("/login", (LoginRequest request, IJwtTokenService tokenService) =>
            {
                // SUPER SIMPLE: hardcoded user for the assignment.
                // You can change this to check MongoDB or something later.
                if (request.Username == "student" && request.Password == "password123")
                {
                    var token = tokenService.GenerateToken(request.Username, "Student");

                    return Results.Ok(new
                    {
                        token
                    });
                }

                return Results.Unauthorized();
            })
            .WithName("Login")
            .WithSummary("Gets a JWT token for a valid user.")
            .WithDescription("Send username/password and receive a JWT token for calling protected endpoints.")
            .AllowAnonymous();

            return app;
        }
    }
}

