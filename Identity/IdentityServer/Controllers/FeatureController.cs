using ApiClient.IdentityServer.Models.RequestBodies;
using IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.ApiBuilder;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class FeatureController : ApiController
{
    private readonly IFeatureService _featureService;
    
    public FeatureController(ILogger<FeatureController> logger, IFeatureService featureService) 
        : base(logger)
    {
        _featureService = featureService;
    }
}