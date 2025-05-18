using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["JwtBearer:Authority"];
        options.Audience = builder.Configuration["JwtBearer:Audience"];
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("AUTH FAILED: " + ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine("AUTH CHALLENGE: " + ctx.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddOpenApi();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("time", policy =>
        policy.RequireAuthenticatedUser());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();

app.MapGet("/time", () =>
{
    return new
    {
        CurrentTime = DateTime.UtcNow,
        TimeZone = "UTC"
    };
}).RequireAuthorization("time");

app.MapGet("/test", () => "This should be public").AllowAnonymous();

app.Run();
