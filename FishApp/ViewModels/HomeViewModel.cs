using System.Collections.Generic;

namespace FishApp.ViewModels;

/// <summary>
/// Steuert die Startseite.
/// </summary>
public class HomeViewModel : BaseViewModel
{
    private readonly List<HomeNavigationItem> _items = new()
    {
        new("Meine Aquarien", "aquariums"),
        new("Fisch-Datenbank", "fishdb"),
        new("Suche", "search")
    };

    public IReadOnlyList<HomeNavigationItem> NavigationItems => _items;
}

public record HomeNavigationItem(string Title, string Route);
