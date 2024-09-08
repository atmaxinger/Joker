using Joker.Services.Jokes;
using Joker.Services.Jokes.JokeApi;
using Joker.Services.Settings;
using Joker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Joker;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddHttpClient();
        collection.AddTransient<IJokeProvider, JokeApiJokeProvider>();
        collection.AddSingleton<ISettingsService, SettingsService>();
        collection.AddTransient<MainWindowViewModel>();
    }
}