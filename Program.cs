using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
var configuration = builder.Configuration;

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

// Add authorization middleware
builder.Services.AddAuthorization();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Run the application
app.Run();