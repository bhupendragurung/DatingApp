using Api.Common.Auth;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Common.Middleware;

public sealed class LastActiveMiddleware(RequestDelegate next)
{
    private static readonly TimeSpan UpdateInterval = TimeSpan.FromMinutes(1);

    public async Task InvokeAsync(HttpContext context, AppDbContext db, TimeProvider timeProvider)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.GetUserId();
            var now = timeProvider.GetUtcNow().UtcDateTime;
            var threshold = now - UpdateInterval;

            await db.Users
                .Where(u => u.Id == userId && u.LastActive < threshold)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(u => u.LastActive, now),
                    context.RequestAborted);
        }

        await next(context);
    }
}