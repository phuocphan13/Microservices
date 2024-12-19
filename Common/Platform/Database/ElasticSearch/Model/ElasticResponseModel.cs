namespace Platform.Database.ElasticSearch.Model;

public class ElasticResponseModel
{
    public string Id { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int SeverityNumber { get; set; }
    public string SeverityText { get; set; } = null!;
    public string SpanId { get; set; } = null!;
    public int TraceFlags { get; set; }
    public string TraceId { get; set; } = null!;
    
    public ElasticResourceModel Resource { get; set; } = null!; 
    public ElasticScopeModel Scope { get; set; } = null!;
}