using System.Data.SqlClient;
using System.Text.Json;
using Dapper;
using GamePlatformRepo.Core.Data;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly GamePlatformDbContext dbContext;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, GamePlatformDbContext dbContext)
    {
        _next = next;
        _logger = logger;
        this.dbContext = dbContext;
    }

    public async Task Invoke(HttpContext context)
    {
        var logEntry = new EntryLog
        {
            Url = context.Request.Path,
            RequestHeaders = JsonSerializer.Serialize(context.Request.Headers),
            MethodType = context.Request.Method,
            ClientIp = context.Connection.RemoteIpAddress?.ToString(),
            CreationDateTime = DateTime.UtcNow
        };

        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            logEntry.RequestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);

        logEntry.StatusCode = context.Response.StatusCode;
        logEntry.ResponseHeaders = JsonSerializer.Serialize(context.Response.Headers);
        logEntry.EndDateTime = DateTime.UtcNow;

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        logEntry.ResponseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        context.Response.Body = originalResponseBodyStream;

        await SaveLogToDatabase(logEntry);
    }

    private async Task SaveLogToDatabase(EntryLog log)
    {
        var sql = @"
            INSERT INTO Logs ( Url, RequestBody, RequestHeaders, MethodType, ResponseBody, ResponseHeaders, StatusCode, CreationDateTime, EndDateTime, ClientIp)
            VALUES ( @Url, @RequestBody, @RequestHeaders, @MethodType, @ResponseBody, @ResponseHeaders, @StatusCode, @CreationDateTime, @EndDateTime, @ClientIp)";

        using var connection = new SqlConnection("Server=localhost;Database=GamePlatformDb;Integrated Security=true;");
        await connection.ExecuteAsync(sql, log);
    }
}
