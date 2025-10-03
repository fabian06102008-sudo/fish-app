using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class FishDatabasePage : ContentPage
{
    private static FishDatabaseViewModel? _viewModel;

    public FishDatabasePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    public static void SetViewModel(FishDatabaseViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext ??= _viewModel;
    }
}
