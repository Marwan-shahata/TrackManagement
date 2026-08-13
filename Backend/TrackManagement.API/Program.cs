using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using TrackManagement.API.ExceptionHandling;
using TrackManagement.API.OpenApi;
using TrackManagement.Application.Interfaces.Repositories;
using TrackManagement.Application.Interfaces.Services;
using TrackManagement.Application.Services;
using TrackManagement.Infrastructure.Authentication;
using TrackManagement.Infrastructure.Data;
using TrackManagement.Infrastructure.Data.Seed;
using TrackManagement.Infrastructure.Repositories;
using TrackManagement.API.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// ==========================
// Controllers + OpenAPI
// ==========================

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<
        BearerSecuritySchemeTransformer>();
});
// ==========================
// Database
// ==========================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options
        .UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"))

        .UseSeeding((context, _) =>
        {
            DatabaseSeeder.Seed(
                (ApplicationDbContext)context);
        })

        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            await DatabaseSeeder.SeedAsync(
                (ApplicationDbContext)context,
                cancellationToken);
        });
});

// ==========================
// Repositories
// ==========================

builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IDspRepository, DspRepository>();

// ==========================
// Services
// ==========================

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ==========================
// Exception Handling
// ==========================

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ==========================
// JWT Authentication
// ==========================

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is not configured.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidateAudience = true,

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

// ==========================
// Build
// ==========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ==========================
// Automatic Migration
// ==========================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

// ==========================
// Development
// ==========================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ==========================
// Middleware
// ==========================


app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
