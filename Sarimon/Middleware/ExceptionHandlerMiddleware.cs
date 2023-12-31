﻿namespace Curacaru.Backend.Middleware;

using System.Net;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

/// <summary>Middleware to handle exceptions.</summary>
public class ExceptionHandlerMiddleware : IMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try { await next(context); }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "text";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (BadRequestException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text";

                if (pex.SqlState == "23503")
                    await context.Response.WriteAsync("Eintrag ist noch in Verwendung ist.");
                else
                    await context.Response.WriteAsync(pex.MessageText);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text";
                await context.Response.WriteAsync(ex.Message);
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text";
            await context.Response.WriteAsync(ex.Message);
        }
    }
}