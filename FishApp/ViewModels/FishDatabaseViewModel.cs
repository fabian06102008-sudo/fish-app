using System.Collections.ObjectModel;
using System.Windows.Input;
using FishApp.Models;
using FishApp.Services;
using Microsoft.Maui.Controls;

namespace FishApp.ViewModels;

/// <summary>
/// ViewModel für die Fisch-Datenbank.
/// </summary>
public class FishDatabaseViewModel : BaseViewModel
{
    private readonly FishRepository _repository;
    private string _searchQuery = string.Empty;

    public ObservableCollection<Fish> FishItems { get; }

    public ICommand SearchCommand { get; }

    public FishDatabaseViewModel(FishRepository repository)
    {
        _repository = repository;
        FishItems = new ObservableCollection<Fish>(_repository.FishList);
        SearchCommand = new Command<string>(ExecuteSearch);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            SetProperty(ref _searchQuery, value);
            ExecuteSearch(value);
        }
    }

    private void ExecuteSearch(string? query)
    {
        FishItems.Clear();
        foreach (var fish in _repository.Search(query ?? string.Empty))
        {
            FishItems.Add(fish);
        }
    }
}
