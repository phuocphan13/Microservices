using Newtonsoft.Json.Linq;

namespace Core.Common.Helpers;

public static class JsonHelpers
{
    public static JObject DeserializeFromBase64(string value)
    {
        var decoded = Convert.FromBase64String(value);
        var decodedPayload = System.Text.Encoding.Default.GetString(decoded);
        
        return JObject.Parse(decodedPayload);
    }
}