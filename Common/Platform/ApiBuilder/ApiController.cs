using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Platform.ApiBuilder;

[ApiController]
public abstract partial class ApiController : Controller
{
    public ILogger<ApiController> Logger { get; }

    protected ApiController(ILogger<ApiController> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        Logger = logger;
    }

    #region Return Ok Result
    protected new OkObjectResult Ok()
    {
        return base.Ok(this.CreateApiStatusResult(HttpStatusCode.OK, "OK", 0));
    }

    protected OkObjectResult Ok<T>(T? value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        ApiResult apiResult1 = null!;

        if (value is ApiResult apiResult2)
        {
            apiResult1 = apiResult2;
        }

        if (apiResult1 == null)
        {
            apiResult1 = this.CreateApiDataResult(HttpStatusCode.OK, value);
        }

        return base.Ok(apiResult1);
    }

    protected OkObjectResult Ok<T>(List<T> value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        ApiResult apiResult1 = this.CreateApiCollectionResult(HttpStatusCode.OK, value);

        return base.Ok(apiResult1);
    }
    #endregion

    #region Return NotFound Result
    protected new ObjectResult NotFound()
        => this.NotFound<object>(null);

    protected ObjectResult NotFound<T>(T? additional)
        => this.NotFound("Not found.", additional, 0);

    protected ObjectResult NotFound(string message)
        => this.NotFound(message, string.Empty, 0);

    protected ObjectResult NotFound<T>(string message, T additional)
        => this.NotFound(message, additional, 0);

    protected ObjectResult NotFound<T>(string message, T? additional, int errorCode)
        => base.NotFound(this.CreateApiStatusResult(HttpStatusCode.NotFound, message, additional, errorCode));

    protected new ObjectResult NotFound(object value)
        => this.NotFound<object>(value);
    #endregion

    #region Return Bad Request Result
    protected new ObjectResult BadRequest()
        => this.BadRequest<object>(null!);

    protected ObjectResult BadRequest<T>(T additional)
        => this.BadRequest("Bad request.", additional, 0);
    
    protected ObjectResult BadRequest(string message)
        => this.BadRequest(message, string.Empty, 0);

    protected ObjectResult BadRequest<T>(string message, T additional)
        => this.BadRequest(message, additional, 0);

    protected ObjectResult BadRequest<T>(string message, T additional, int errorCode)
        => base.BadRequest(this.CreateApiStatusResult(HttpStatusCode.BadRequest, message, additional, errorCode));

    protected new ObjectResult BadRequest(object error)
        => this.BadRequest<object>(error);
    #endregion

    private ApiStatusResult CreateApiStatusResult(HttpStatusCode statusCode, string message, int errorCode)
    {
        ApiStatusResult result = new();

        this.InitializeApiStatusResult(result, statusCode, message, errorCode);

        return result;
    }

    private ApiStatusResult<T> CreateApiStatusResult<T>(
        HttpStatusCode statusCode,
        string message,
        T? additional,
        int errorCode)
    {
        ApiStatusResult<T> result = new()
        {
            Additional = additional,
        };

        this.InitializeApiStatusResult(result, statusCode, message, errorCode);

        return result;
    }

    private void InitializeApiStatusResult(
        ApiStatusResult result,
        HttpStatusCode statusCode,
        string message,
        int errorCode)
    {
        result.StatusCode = statusCode;
        result.Message = message;
        result.InternalErrorCode = errorCode;
    }

    private ApiCollectionResult<T> CreateApiCollectionResult<T>(HttpStatusCode statusCode, IEnumerable<T> value)
    {
        ApiCollectionResult<T> apiDataResult = new(value)
        {
            StatusCode = statusCode
        };

        return apiDataResult;
    }

    private ApiDataResult<T> CreateApiDataResult<T>(HttpStatusCode statusCode, T value)
    {
        ApiDataResult<T> apiDataResult = new(value)
        {
            StatusCode = statusCode
        };

        return apiDataResult;
    }
}