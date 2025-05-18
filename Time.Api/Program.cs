using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("time", policy =>
        policy
            .RequireRole("admin")
            .RequireClaim("scope", "time_api"));

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

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
