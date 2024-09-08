using Avalonia.Controls;
using Avalonia.Interactivity;
using Joker.ViewModels;

namespace Joker.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
    }

    private void ApplyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsWindowViewModel viewModel)
        {
            Close(viewModel.BuildJokeOptions());
        }
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is SettingsWindowViewModel viewModel)
        {
            Close(viewModel.OriginalJokeOptions);
        }
    }
}