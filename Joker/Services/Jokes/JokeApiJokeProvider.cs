using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Joker.Helpers;

namespace Joker.Services.Jokes;

file class JokeApiResult
{
    [JsonPropertyName("error")] public bool Error { get; set; }
    
    [JsonPropertyName("message")] public string? Message { get; set; }
    
    [JsonPropertyName("joke")] public string? Joke { get; set; }

    [JsonPropertyName("category")] public string? Category { get; set; }
}

public class JokeApiJokeProvider : IJokeProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JokeApiJokeProvider(IHttpClientFactory httpClient)
    {
        _httpClientFactory = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<Joke, string>> GetJokeAsync(JokeOptions jokeOptions)
    {
        using var client = _httpClientFactory.CreateClient();

        var url = "https://v2.jokeapi.dev/joke/";

        if (jokeOptions.Categories.Length > 0)
        {
            url += string.Join(",", jokeOptions.Categories);
        }
        else
        {
            url += "Any";
        }
        
        url += "?type=single";
        
        if (jokeOptions.OnlySafeJokes)
        {
            url += "&blacklistFlags=nsfw,religious,political,racist,sexist,explicit";
        }

        var apiResult = await client.GetAsync(url);
        if (!apiResult.IsSuccessStatusCode) return Result<Joke, string>.Failure("Unable to fetch Joke");

        var content = await apiResult.Content.ReadFromJsonAsync<JokeApiResult>();
        if (content is null) return Result<Joke, string>.Failure("Unable to decode Joke");
        
        if (content.Error) return Result<Joke, string>.Failure(content.Message ?? "Unknown Error");

        return Result<Joke, string>.Success(new Joke(content.Joke ?? "", content.Category ?? ""));
    }
}