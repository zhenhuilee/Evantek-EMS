// Using Directives 
// These bring in necessary namespaces and libraries for the application
using EMS.DAL.Implementation;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using EMS.Mapper.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using EMS.DTO;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EMS.API.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// WebApplication Builder
// This creates a builder for the web application and configures services
var builder = WebApplication.CreateBuilder(args);


// Authentication Setup
// Configures JWT authentication
// This sets up JWT bearer authentication with token validation parameters using configuration settings for issuer, audience, and signing key.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

// Service Registration
// Add various services to the DI container
// Add services to the container

// AutoMapper: Adds AutoMapper with a specific profile for object mapping.
builder.Services.AddAutoMapper(typeof(UserProfile));

// DbContext: Registers the EmsDbContext for Entity Framework Core with SQL Server.
builder.Services.AddDbContext<EmsDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlDbConnection");
    options.UseSqlServer(connectionString,
                         sqlOptionsBuilder => sqlOptionsBuilder.EnableRetryOnFailure());
});


builder.Services.AddControllers();

builder.Services.AddSignalR();

// Swagger: Registers Swagger for API documentation.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddOptions();



// Dependency controller
// Registers the Data Access Layer (DAL) implementations and interfaces
// Dependency Injection for DAL classes and interfaces
builder.Services.AddScoped<IUserMgr, UserMgr>();
builder.Services.AddScoped<ILoginMgr, LoginMgr>();
builder.Services.AddScoped<IAttendanceMgr, AttendanceMgr>();
builder.Services.AddScoped<IDisplayMgr, DisplayMgr>();
builder.Services.AddScoped<IChangePasswordMgr, ChangePasswordMgr>();
builder.Services.AddScoped<IModuleMgr, ModuleMgr>();
builder.Services.AddScoped<IAuditMgr, AuditMgr>();
builder.Services.AddScoped<IStatusMgr, StatusMgr>();
builder.Services.AddScoped<IIncidentMgr, IncidentMgr>();
builder.Services.AddScoped<IEmailMgr, EmailMgr>();



builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "jwt",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

// CORS Policy
// Configure CORS to allow requests from a specific origin
var corsIps = (builder.Configuration["WebUI_IP"] ?? "").Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

builder.Services.AddCors(options =>
    options.AddPolicy("CORSPolicy",
    policy =>
    {
        policy
            .WithOrigins(corsIps.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
        .AllowCredentials();
    })
);

// Application Build and Middleware Configuration

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CORSPolicy");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.MapHub<DisplayHub>("/displayHub");

app.MapControllers();

app.Run();