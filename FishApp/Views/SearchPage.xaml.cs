using System.Collections.Generic;
using System.Threading.Tasks;
using FishApp.Models;
using FishApp.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class SearchPage : ContentPage
{
    private static SearchViewModel? _viewModel;

    public SearchPage()
    {
        InitializeComponent();
        AttachViewModel();
    }

    public static void SetViewModel(SearchViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AttachViewModel();
    }

    private Task NavigateToDetailAsync(Fish fish)
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "Fish", fish }
            };
            await Shell.Current.GoToAsync(nameof(FishDetailPage), parameters);
            _viewModel!.SelectedFish = null;
        });
    }

    private void AttachViewModel()
    {
        if (_viewModel is null)
        {
            return;
        }

        BindingContext = _viewModel;
        _viewModel.FishSelected -= NavigateToDetailAsync;
        _viewModel.FishSelected += NavigateToDetailAsync;
    }
}
