using System;
using System.Windows.Input;
using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class HomePage : ContentPage
{
    private static HomeViewModel? _viewModel;

    public ICommand NavigateCommand { get; }

    public HomePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
        NavigateCommand = new Command<string>(async route =>
        {
            if (!string.IsNullOrWhiteSpace(route))
            {
                var targetRoute = route.StartsWith("//", StringComparison.Ordinal) ? route : $"//{route}";
                await Shell.Current.GoToAsync(targetRoute);
            }
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext ??= _viewModel;
    }

    public static void SetViewModel(HomeViewModel viewModel)
    {
        _viewModel = viewModel;
    }
}
