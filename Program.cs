using DotNetEnv;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
Env.Load();
var jwtIssuer = Env.GetString("JWT_ISSUER") ?? "your-backend.com";
var jwtAudience = Env.GetString("JWT_AUDIENCE") ?? "your-frontend.com";
var jwtKey = Env.GetString("JWT_SECRET");
var dbPassword = Env.GetString("DB_PASSWORD") ?? "";

if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT_SECRET is missing from environment variables");
}

// Load MySQL connection string from appsettings.json
var connectionString = $"Server=localhost;Database=finance_tracker;User=root;Password={dbPassword};";

// Configure MySQL Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
//Add CORS for browsers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();