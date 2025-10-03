using FishApp.Services;
using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp;

public partial class App : Application
{
    public static JsonStorageService StorageService { get; } = new();
    public static FishRepository FishRepository { get; } = new();

    public App()
    {
        InitializeComponent();

        // Lade Daten synchron, bevor das UI aufgebaut wird
        StorageService.LoadAsync().GetAwaiter().GetResult();

        // Registriere zentrale Services und ViewModels
        var homeViewModel = new HomeViewModel();
        var aquariumsViewModel = new AquariumsViewModel(StorageService, FishRepository);
        var fishDbViewModel = new FishDatabaseViewModel(FishRepository);
        var searchViewModel = new SearchViewModel(FishRepository);

        MainPage = new AppShell(
            homeViewModel,
            aquariumsViewModel,
            fishDbViewModel,
            searchViewModel);
    }
}
