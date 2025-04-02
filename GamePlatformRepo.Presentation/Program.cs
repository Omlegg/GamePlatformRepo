using System.Security.Claims;
using FluentValidation;
using GamePlatformRepo.Core.Data;
using GamePlatformRepo.Core.Models;
using GamePlatformRepo.InfraStructure.Repositories;
using GamePlatformRepo.Middlewares;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;
using GamePlatformRepo.Repository.Base;
using GamePlatformRepo.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {}).AddEntityFrameworkStores<GamePlatformDbContext>();

builder.Services.AddDataProtection();


builder.Services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme,
        configureOptions: options =>
        {
            options.LoginPath = "/User/Login";
        });


builder.Services.AddAuthorization(options => {
    options.AddPolicy(
        name: "MyPolicy",
        configurePolicy: policyBuilder => {
            policyBuilder
                .RequireRole("Admin")
                .RequireClaim(ClaimTypes.Name, "Omlegg")
                .RequireRole("User");
        }
    );
});


builder.Services.AddScoped<IGameRepository, GameEFRepository>();
builder.Services.AddScoped<ICommentRepository, CommentEFRepository>();

builder.Services.AddScoped<IValidator<Game>, GameValidator>();
builder.Services.AddScoped<IValidator<Comment>, CommentValidator>();
builder.Services.AddDbContext<GamePlatformDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


var serviceScope = app.Services.CreateScope();
var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

await roleManager.CreateAsync(new IdentityRole {Name = "Admin"});
await roleManager.CreateAsync(new IdentityRole {Name = "User"});

app.Run();
