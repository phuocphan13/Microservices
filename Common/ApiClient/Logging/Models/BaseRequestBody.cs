namespace ApiClient.Logging.Models;

public class BaseRequestBody
{
    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int Type { get; set; }

    public int Meter { get; set; }

    public string ObjectName { get; set; } = null!;
}