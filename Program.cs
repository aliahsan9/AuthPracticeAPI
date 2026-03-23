using AuthPracticeAPI.Data;
using AuthPracticeAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters

     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidateIssuer = jwtSettings["Issuer"],
         ValidateAudience = jwtSettings["Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? string.Empty)) 


        };
 });


// Configure Db
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
