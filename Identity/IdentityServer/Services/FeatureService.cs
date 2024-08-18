using ApiClient.IdentityServer.Models.RequestBodies;
using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using IdentityServer.Extensions.TransformExtensions;
using Microsoft.EntityFrameworkCore;
using Platform.Common.Session;

namespace IdentityServer.Services;

public interface IFeatureService
{
    Task<FeatureDetail?> CreateFeatureAsync(CreateFeatureRequestBody requestBody, CancellationToken cancellationToken = default);
    Task<bool> RemoveFeatureAsync(Guid featureId, CancellationToken cancellationToken = default);
}

public class FeatureService : IFeatureService
{
    private readonly AuthenContext _authenContext;
    private readonly ISessionState _sessionState;

    public FeatureService(AuthenContext authenContext, ISessionState sessionState)
    {
        _authenContext = authenContext;
        _sessionState = sessionState;
    }

    public async Task<FeatureDetail?> CreateFeatureAsync(CreateFeatureRequestBody requestBody, CancellationToken cancellationToken)
    {
        var application = await _authenContext.Application
            .Where(x => x.ExternalId == Guid.Parse(requestBody.ApplicationId))
            .Include(x => x.Features)
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null)
        {
            return null;
        }
        
        var isExisted = application.Features.Any(x => x.Name == requestBody.Name);

        if (isExisted)
        {
            return null;
        }
        
        var feature = new Feature()
        {
            Name = requestBody.Name,
            Description = requestBody.Description,
            ApplicationId = application.Id,
            ExternalId = Guid.NewGuid(),
            CreatedBy = _sessionState.GetUserId(),
            CreatedDate = DateTime.UtcNow
        };

        await _authenContext.Feature.AddAsync(feature, cancellationToken);
        await _authenContext.SaveChangesAsync(cancellationToken);

        return feature.ToDetail(application.Name);
    }

    public async Task<bool> RemoveFeatureAsync(Guid featureId, CancellationToken cancellationToken)
    {
        var feature = await _authenContext.Feature
            .Where(x => x.ExternalId == featureId)
            .FirstOrDefaultAsync(cancellationToken);

        if (feature is null)
        {
            return false;
        }

        _authenContext.Feature.Remove(feature);
        await _authenContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}