using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Joker.Services.Jokes;

namespace Joker.ViewModels;

public partial class JokeCategoryViewModel : ViewModelBase
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _isEnabled;

    public JokeCategoryViewModel(string name, bool isEnabled)
    {
        Name = name;
        IsEnabled = isEnabled;
    }
}

public partial class SettingsWindowViewModel : ViewModelBase
{
    public JokeOptions OriginalJokeOptions { get; }
    
    [ObservableProperty] private bool _onlySafeJokes;
    [ObservableProperty] private List<JokeCategoryViewModel> _categories;
    
    public SettingsWindowViewModel(JokeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        OriginalJokeOptions = options;
        OnlySafeJokes = options.OnlySafeJokes;
        
        var cats = new HashSet<string>(options.Categories);
        cats.Add("Programming");
        cats.Add("Miscellaneous");
        cats.Add("Dark");
        cats.Add("Pun");
        cats.Add("Spooky");
        cats.Add("Christmas");

        var allJokesEnabled = options.Categories.Length == 0;

        Categories = cats
            .Select(c => new JokeCategoryViewModel(c, allJokesEnabled))
            .OrderBy(c => c.Name)
            .ToList();
        
        foreach (var cat in Categories)
        {
            if (options.Categories.Contains(cat.Name))
            {
                cat.IsEnabled = true;
            }
        }
    }
    
    public JokeOptions BuildJokeOptions()
    {
        return OriginalJokeOptions with
        {
            OnlySafeJokes = OnlySafeJokes,
            Categories = Categories.Where(c => c.IsEnabled).Select(c => c.Name).ToArray()
        };
    }
}