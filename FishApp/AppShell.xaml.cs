using FishApp.ViewModels;
using FishApp.Views;
using Microsoft.Maui.Controls;

namespace FishApp;

public partial class AppShell : Shell
{
    public AppShell(
        HomeViewModel homeViewModel,
        AquariumsViewModel aquariumsViewModel,
        FishDatabaseViewModel fishDatabaseViewModel,
        SearchViewModel searchViewModel)
    {
        HomePage.SetViewModel(homeViewModel);
        AquariumsPage.SetViewModel(aquariumsViewModel);
        FishDatabasePage.SetViewModel(fishDatabaseViewModel);
        SearchPage.SetViewModel(searchViewModel);

        InitializeComponent();

        // Hinterlege ViewModel-Instanzen für die Views
        Routing.RegisterRoute(nameof(FishDetailPage), typeof(FishDetailPage));
    }
}
