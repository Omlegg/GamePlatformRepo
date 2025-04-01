using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GamePlatformRepo.Exceptions;
using GamePlatformRepo.Responses;
using Microsoft.AspNetCore.Http;

namespace GamePlatformRepo.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new NotFoundResponse(ex.Key, ex.Message));
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new BadRequestResponse(ex.Message, ex.ParamName));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new SystemErrorResponse(ex.Message ?? "Unhandled Internal Server Error"));
            }
        }
    }

}