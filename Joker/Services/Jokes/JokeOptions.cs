using System.Text.Json.Serialization;

namespace Joker.Services.Jokes;

public record JokeOptions
{
    public bool OnlySafeJokes { get; init; }
    public string[] Categories { get; set; } = [];
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(JokeOptions))]
internal partial class JokeOptionsSourceGenerationContext : JsonSerializerContext
{
}