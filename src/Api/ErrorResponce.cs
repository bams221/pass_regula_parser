using System.Text.Json.Serialization;

namespace PassRegulaParser.Api;

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}