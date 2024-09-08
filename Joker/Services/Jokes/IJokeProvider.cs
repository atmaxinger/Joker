using System.Threading.Tasks;
using Joker.Helpers;

namespace Joker.Services.Jokes;

public interface IJokeProvider
{
    Task<Result<Joke, string>> GetJokeAsync(JokeOptions jokeOptions);
}