using Core.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Platform.Common.Session;

public interface ISessionState
{
    string GetAccessToken();
    string GetUserIdAsync();
}

public class SessionState : ISessionState
{
    private const string SessionVariable_ObjectId = "oid";
    private const string SessionVariable_LastActivity = "la";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private JObject _payloadJObject = null!;

    private bool isLoaded;
    private ISession? session;
    private bool hasChanged;
    private DateTime? lastActivity;

    public SessionState(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetAccessToken()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        var authorizationToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(authorizationToken))
        {
            // throw new InvalidOperationException();
        }

        return authorizationToken;
    }

    public string GetUserIdAsync()
    {
        this.LoadAsync();
        return _payloadJObject.GetValue("userId")!.ToString();
    }

    private void LoadAsync()
    {
        if (this.isLoaded &&
            this.session is not null)
        {
            return;
        }

        this.isLoaded = true;
        this.hasChanged = false;

        GetPrincipal();

        // if (_payloadJObject is null)
        // {
        //     return;
        // }

        // string? objectId = this.session?.GetString(SessionState.SessionVariable_ObjectId);
        //
        // if (string.IsNullOrWhiteSpace(objectId))
        //     // ||
        //     // objectId != this.principal.Id)
        // {
        //     this.Clear();
        //     await this.CommitAsync(cancellationToken);
        // }
        //
        // string lastActivityString = this.session?.GetString(SessionState.SessionVariable_LastActivity) ?? string.Empty;
        // this.lastActivity = null;
        //
        // if (!string.IsNullOrWhiteSpace(lastActivityString) &&
        //     DateTime.TryParseExact(
        //         lastActivityString,
        //         "o",
        //         CultureInfo.InvariantCulture,
        //         DateTimeStyles.RoundtripKind,
        //         out DateTime lastActivityValue))
        // {
        //     this.lastActivity = lastActivityValue;
        // }
    }

    private void GetPrincipal()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        var header = _httpContextAccessor.HttpContext.Request.Headers;
        var payload = RequestHeaderHelper.GetPayloadToken(header);
        _payloadJObject = JsonHelpers.DeserializeFromBase64(payload);
    }

    private async Task CommitAsync(
        CancellationToken cancellationToken)
    {
        this.LoadAsync();

        if (this.hasChanged && this.session is not null)
        {
            string lastActivityString = this.lastActivity?.ToString("o") ?? string.Empty;

            this.session.SetString(SessionState.SessionVariable_LastActivity, lastActivityString);

            this.hasChanged = false;
            await this.session.CommitAsync(cancellationToken);
        }
    }

    private void Clear()
    {
        if (this.session is null &&
            _httpContextAccessor.HttpContext is not null)
        {
            this.session = _httpContextAccessor.HttpContext.Session;
        }

        this.lastActivity = DateTime.MinValue;
        this.hasChanged = true;

        this.session?.Clear();
    }
}