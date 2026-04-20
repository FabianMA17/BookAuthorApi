using BookAuthorApi.Application.Queries.Auth;
using BookAuthorApi.Domain.Entities;
using BookAuthorApi.Domain.Interfaces;
using BookAuthorApi.Domain.Services;
using BookAuthorApi.Infrastructure.Data;
using BookAuthorApi.Infrastructure.Repositories;
using BookAuthorApi.Infrastructure.Services;
using BookAuthorApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Exception handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

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

// EF Core - SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginQuery).Assembly));

// DI
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IIsbnValidator, IsbnValidator>();
builder.Services.AddScoped<IBookCoverService, BookCoverService>();

// HttpClient for external API calls
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed users if not exist
    if (!db.Users.Any(u => u.Username == "user1"))
    {
        db.Users.Add(new User { Username = "user1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password1") });
    }
    if (!db.Users.Any(u => u.Username == "user2"))
    {
        db.Users.Add(new User { Username = "user2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password2") });
    }
    if (!db.Users.Any(u => u.Username == "user3"))
    {
        db.Users.Add(new User { Username = "user3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password3") });
    }
    db.SaveChanges();
}

app.Run();
