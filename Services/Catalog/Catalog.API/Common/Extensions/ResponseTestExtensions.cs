namespace Catalog.API.Common.Extensions;

public class ResponseTestExtensions
{
    private readonly RequestDelegate requestDelegate;

    public ResponseTestExtensions(RequestDelegate requestDelegate)
    {
        this.requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.OnStarting(() =>
        {
            httpContext.Response.Headers["Lucifer"] = "CustomLucifer";
            return Task.CompletedTask;
        });

        
        await requestDelegate(httpContext);
    }
}

public static class ResponseTestMiddlewares
{
    public static void UseMyCustomMiddleware(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ResponseTestExtensions>();
    }
}