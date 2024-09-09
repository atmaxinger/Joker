using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Joker.ViewModels;
using Joker.Views;

namespace Joker;

public class ViewLocator : IDataTemplate
{
    private static readonly Dictionary<Type, Func<Control>> Registrations = new();
    
    public static void Register<TViewModel, TView>() where TView : Control, new()
    {
        Registrations.Add(typeof(TViewModel), () => new TView());
    }

    public static void Register<TViewModel, TView>(Func<TView> factory) where TView : Control
    {
        Registrations.Add(typeof(TViewModel), factory);
    }

    static ViewLocator()
    {
        Register<MainWindow, MainWindow>();
        Register<SettingsWindowViewModel, SettingsWindow>();
        Register<JokeHistoryViewModel, JokeHistoryWindow>();
    }
    
    public Control? Build(object? data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var type = data.GetType();
    
        if (Registrations.TryGetValue(type, out var factory)) {
            return factory();
        }
        else {
            return new TextBlock { Text = "Not Found: " + type };
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}