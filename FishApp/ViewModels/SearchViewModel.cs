using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FishApp.Models;
using FishApp.Services;
using Microsoft.Maui.Controls;

namespace FishApp.ViewModels;

/// <summary>
/// ViewModel für die Suchseite.
/// </summary>
public class SearchViewModel : BaseViewModel
{
    private readonly FishRepository _repository;
    private string _query = string.Empty;
    private Fish? _selectedFish;

    public ObservableCollection<Fish> Results { get; } = new();

    public ICommand SearchCommand { get; }

    public event Func<Fish, Task>? FishSelected;

    public SearchViewModel(FishRepository repository)
    {
        _repository = repository;
        SearchCommand = new Command(ExecuteSearch);
        ExecuteSearch();
    }

    public string Query
    {
        get => _query;
        set
        {
            SetProperty(ref _query, value);
            ExecuteSearch();
        }
    }

    public Fish? SelectedFish
    {
        get => _selectedFish;
        set
        {
            SetProperty(ref _selectedFish, value);
            if (value != null)
            {
                _ = FishSelected?.Invoke(value);
            }
        }
    }

    private void ExecuteSearch()
    {
        Results.Clear();
        foreach (var fish in _repository.Search(Query))
        {
            Results.Add(fish);
        }
    }
}
