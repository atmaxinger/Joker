namespace Joker.Services.Jokes;

public record JokeOptions
{
    public bool OnlySafeJokes { get; init; }
    public string[] Categories { get; set; } = [];
}
