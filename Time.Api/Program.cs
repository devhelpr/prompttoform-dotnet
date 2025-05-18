using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["JwtBearer:Authority"];
        options.Audience = builder.Configuration["JwtBearer:Audience"];
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddOpenApi();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("time", policy =>
    policy.RequireAuthenticatedUser());

var app = builder.Build();

app.MapOpenApi().AllowAnonymous(); ;

app.MapScalarApiReference().AllowAnonymous(); ;

app.UseHttpsRedirection();

app.MapGet("/time", () =>
{
    return new
    {
        CurrentTime = DateTime.UtcNow,
        TimeZone = "UTC"
    };
}).RequireAuthorization("time");

app.Run();
