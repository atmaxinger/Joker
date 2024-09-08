using System.Threading.Tasks;
using Joker.Services.Jokes;

namespace Joker.Services.Settings;

public interface ISettingsService
{
    JokeOptions JokeOptions { get; }
    
    Task InitializeAsync();
    
    Task<JokeOptions> ShowJokeOptionsDialogAsync();
}