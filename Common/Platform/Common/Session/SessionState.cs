using System.Globalization;
using Microsoft.AspNetCore.Http;
using Platform.Common.Security;

namespace Platform.Common.Session;

public interface ISessionState
{
    string GetAccessToken();
    Task<string> GetUserIdAsync(CancellationToken cancellationToken = default);
}

public class SessionState : ISessionState
{
    private const string SessionVariable_ObjectId = "oid";
    private const string SessionVariable_LastActivity = "la";
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Principal principal = null!;

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
        if(_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }
        
        var authorizationToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(authorizationToken))
        {
            throw new InvalidOperationException();
        }

        return authorizationToken;
    }
    
    public async Task<string> GetUserIdAsync(CancellationToken cancellationToken)
    {
        await this.LoadAsync(cancellationToken);
        return this.principal.Id;
    }

    private async ValueTask LoadAsync(
        CancellationToken cancellationToken)
    {
        if (this.isLoaded &&
            this.session is not null)
        {
            return;
        }

        this.isLoaded = true;
        this.hasChanged = false;

        await this.GetSession(cancellationToken);

        if (!this.principal.IsAuthenticated)
        {
            return;
        }

        string? objectId = this.session?.GetString(SessionState.SessionVariable_ObjectId);

        if (string.IsNullOrWhiteSpace(objectId) ||
            objectId != this.principal.Id)
        {
            this.Clear();
            await this.CommitAsync(cancellationToken);
        }

        string lastActivityString = this.session?.GetString(SessionState.SessionVariable_LastActivity) ?? string.Empty;
        this.lastActivity = null;
        
        if (!string.IsNullOrWhiteSpace(lastActivityString) &&
            DateTime.TryParseExact(
                lastActivityString,
                "o",
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind,
                out DateTime lastActivityValue))
        {
            this.lastActivity = lastActivityValue;
        }
    }

    private async ValueTask<ISession> GetSession(CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        this.GetPrincipal();

        if (this.session is null)
        {
            this.session = _httpContextAccessor.HttpContext.Session;
            await this.session.LoadAsync(cancellationToken);
        }

        return this.session;
    }

    private Principal GetPrincipal()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        this.principal ??= new Principal(_httpContextAccessor.HttpContext.User);

        return this.principal;
    }

    private async Task CommitAsync(
        CancellationToken cancellationToken)
    {
        await this.LoadAsync(cancellationToken);

        if (this.hasChanged && this.session is not null)
        {
            string lastActivityString = this.lastActivity?.ToString("o") ?? string.Empty;
            
            this.session.SetString(SessionState.SessionVariable_ObjectId, this.principal.Id);
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