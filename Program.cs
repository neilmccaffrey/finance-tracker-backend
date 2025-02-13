using DotNetEnv;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
Env.Load();

// JWT configuration
var jwtIssuer = Env.GetString("JWT_ISSUER") ?? "your-backend.com";
var jwtAudience = Env.GetString("JWT_AUDIENCE") ?? "your-frontend.com";
var jwtKey = Env.GetString("JWT_SECRET");

if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT_SECRET is missing from environment variables");
}

// MySQL connection string
var connectionString = Environment.GetEnvironmentVariable("MYSQL_URL");

// Configure MySQL Database Connection
try
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));
    });
    Console.WriteLine("Database connection configured.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring database: {ex.Message}");
    throw; // Ensure the exception is thrown if it happens
}

// JWT Authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Add CORS for browsers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000"; // Default to 10000 if PORT is not set

// Configure the application to listen on the port provided by Render
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, int.Parse(port)); // Use the dynamic port
});

var app = builder.Build();

// Middleware pipeline
app.UseCors("AllowAll");  // Apply CORS policy
app.UseAuthentication();  // Authentication middleware
app.UseAuthorization();   // Authorization middleware

app.MapControllers();  // Map API controllers

app.Run();  // Run the app