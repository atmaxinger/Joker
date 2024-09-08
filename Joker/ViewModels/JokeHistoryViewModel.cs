using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Joker.Services.Jokes;

namespace Joker.ViewModels;

public partial class JokeHistoryViewModel : ViewModelBase
{
    [ObservableProperty] List<Joke> _jokes;

    public JokeHistoryViewModel(JokeHistory jokeHistory)
    {
        Jokes = jokeHistory.GetJokes().ToList();
    }
}