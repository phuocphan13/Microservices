namespace AngularClient.Middlewares;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        
        return _next(httpContext);
    }
}

public static class TokenMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenMiddleware>();
    }
} 