using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Joker.Services.Jokes;
using Joker.ViewModels;
using Joker.Views;

namespace Joker.Services.Settings;

public class SettingsService : ISettingsService
{
    public JokeOptions JokeOptions { get; private set; } = new()
    {
        OnlySafeJokes = true,
    };

    private string GetSettingsFolderPath()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        path = Path.Combine(path, "at.maxirlinger.Joker");
        return path;
    }

    private string GetSettingsFilePath()
    {
        return Path.Combine(GetSettingsFolderPath(), "settings.json");
    }

    public async Task InitializeAsync()
    {
        var settingsPath = GetSettingsFilePath();
        if (File.Exists(settingsPath))
        {
            await using var settingsFile = File.Open(settingsPath, FileMode.Open, FileAccess.Read);
            JokeOptions = await JsonSerializer.DeserializeAsync(settingsFile, JokeOptionsSourceGenerationContext.Default.JokeOptions) ??
                          new JokeOptions();
        }
    }

    public async Task PersistSettingsAsync()
    {
        var settingsFolder = GetSettingsFolderPath();
        if (!Directory.Exists(settingsFolder))
        {
            Directory.CreateDirectory(settingsFolder);
        }

        await using var settingsFile = File.Open(GetSettingsFilePath(), FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(settingsFile, JokeOptions, JokeOptionsSourceGenerationContext.Default.JokeOptions);
    }

    public async Task<JokeOptions> ShowJokeOptionsDialogAsync()
    {
        var vm = new SettingsWindowViewModel(JokeOptions);
        var window = new SettingsWindow
        {
            DataContext = vm
        };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            JokeOptions = await window.ShowDialog<JokeOptions?>(desktop.MainWindow!) ?? JokeOptions;
            await PersistSettingsAsync();
        }

        return JokeOptions;
    }
}