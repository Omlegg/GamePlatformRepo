using FluentValidation;
using GamePlatformRepo.Middlewares;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;
using GamePlatformRepo.Repository.Base;
using GamePlatformRepo.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IGameRepository, GameDapperRepository>();
builder.Services.AddScoped<ICommentRepository, CommentDapperRepository>();
builder.Services.AddScoped<IValidator<Game>, GameValidator>();
builder.Services.AddScoped<IValidator<Comment>, CommentValidator>();

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



app.Run();
