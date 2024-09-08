using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Joker.Services.Jokes;
using Joker.Services.Settings;

namespace Joker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IJokeProvider _jokeProvider;
    private readonly ISettingsService _settingsService;
    private readonly JokeHistory _jokeHistory = new();
    
    [ObservableProperty] private string _greeting = "Loading Joke ...";
    [ObservableProperty] private string _category = "Loading ...";
    [ObservableProperty] private string _jokeCounter = "";
    
    [ObservableProperty] private bool _isLoading;

    public MainWindowViewModel(IJokeProvider jokeProvider, ISettingsService settingsService)
    {
        _jokeProvider = jokeProvider;
        _settingsService = settingsService;
        NextJokeCommand = new AsyncRelayCommand(LoadJoke);
        OpenSettingsCommand = new AsyncRelayCommand(_settingsService.ShowJokeOptionsDialogAsync);
        PreviousJokeCommand = new RelayCommand(PreviousJoke);
        _ = LoadJoke();
    }

    public ICommand NextJokeCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand PreviousJokeCommand { get; }

    private void SetCurrentJoke(Joke joke)
    {
        OnPropertyChanged(nameof(PreviousJokeCommand));
        
        Greeting = joke.Text;
        Category = joke.Category;
        JokeCounter = $"{_jokeHistory.JokesCount} jokes loaded";
    }
    
    private async Task LoadJoke()
    {
        if (_jokeHistory.TryGetNextJoke(out var nextJoke))
        {
            SetCurrentJoke(nextJoke);
            return;
        }
        
        IsLoading = true;
        try
        {
            var jokeResult = await _jokeProvider.GetJokeAsync(_settingsService.JokeOptions);
            jokeResult.Handle(
                joke =>
                {
                    _jokeHistory.AddJoke(joke);
                    SetCurrentJoke(joke);
                },
                error =>
                {
                    Category = "Error";
                    Greeting = error;
                });
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private void PreviousJoke()
    {
        if (_jokeHistory.TryGetPreviousJoke(out var joke))
        {
            SetCurrentJoke(joke);
        }
    }
}