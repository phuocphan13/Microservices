using System.Net;

namespace Platform.ApiBuilder;

public class ApiResult
{
    public HttpStatusCode StatusCode { get; set; }

    public bool IsSuccessStatusCode => this.StatusCode.IsSuccess();
}