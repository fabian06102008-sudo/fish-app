using System.Threading.Tasks;
using FishApp.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class AquariumsPage : ContentPage
{
    private static AquariumsViewModel? _viewModel;

    public AquariumsPage()
    {
        InitializeComponent();
        AttachViewModel();
    }

    public static void SetViewModel(AquariumsViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AttachViewModel();
    }

    private Task ShowWarningAsync(string title, string message) =>
        MainThread.InvokeOnMainThreadAsync(() => DisplayAlert(title, message, "OK"));

    private Task ShowInfoAsync(string title, string message) =>
        MainThread.InvokeOnMainThreadAsync(() => DisplayAlert(title, message, "OK"));

    private void AttachViewModel()
    {
        if (_viewModel is null)
        {
            return;
        }

        BindingContext = _viewModel;
        _viewModel.WarningRequested -= ShowWarningAsync;
        _viewModel.InfoRequested -= ShowInfoAsync;
        _viewModel.WarningRequested += ShowWarningAsync;
        _viewModel.InfoRequested += ShowInfoAsync;
    }
}
