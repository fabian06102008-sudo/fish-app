using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using FishApp.Models;
using FishApp.Services;

namespace FishApp.ViewModels;

/// <summary>
/// ViewModel zum Verwalten der Aquarien und Bewohner.
/// </summary>
public class AquariumsViewModel : BaseViewModel
{
    private readonly JsonStorageService _storageService;
    private readonly FishRepository _fishRepository;

    private string _newName = string.Empty;
    private int _newVolume = 60;
    private double _newPh = 7.0;
    private int _newGh = 10;
    private int _newKh = 5;
    private double _newTemperature = 25.0;
    private Aquarium? _selectedAquarium;
    private Fish? _selectedFish;

    public ObservableCollection<Aquarium> Aquariums { get; }
    public ObservableCollection<Fish> AvailableFish { get; }

    public event Func<string, string, Task>? WarningRequested;
    public event Func<string, string, Task>? InfoRequested;

    public AquariumsViewModel(JsonStorageService storageService, FishRepository fishRepository)
    {
        _storageService = storageService;
        _fishRepository = fishRepository;

        Aquariums = new ObservableCollection<Aquarium>(_storageService.Aquariums);
        AvailableFish = new ObservableCollection<Fish>(_fishRepository.FishList);

        AddAquariumCommand = new Command(async () => await AddAquariumAsync(), CanAddAquarium);
        AddFishCommand = new Command(async () => await AddFishAsync(), () => SelectedAquarium != null && SelectedFish != null);

        SelectedAquarium = Aquariums.FirstOrDefault();
    }

    public string NewName
    {
        get => _newName;
        set
        {
            SetProperty(ref _newName, value);
            ((Command)AddAquariumCommand).ChangeCanExecute();
        }
    }

    public int NewVolume
    {
        get => _newVolume;
        set
        {
            SetProperty(ref _newVolume, value);
            ((Command)AddAquariumCommand).ChangeCanExecute();
        }
    }

    public double NewPh
    {
        get => _newPh;
        set => SetProperty(ref _newPh, value);
    }

    public int NewGh
    {
        get => _newGh;
        set => SetProperty(ref _newGh, value);
    }

    public int NewKh
    {
        get => _newKh;
        set => SetProperty(ref _newKh, value);
    }

    public double NewTemperature
    {
        get => _newTemperature;
        set => SetProperty(ref _newTemperature, value);
    }

    public Aquarium? SelectedAquarium
    {
        get => _selectedAquarium;
        set
        {
            SetProperty(ref _selectedAquarium, value);
            ((Command)AddFishCommand).ChangeCanExecute();
        }
    }

    public Fish? SelectedFish
    {
        get => _selectedFish;
        set
        {
            SetProperty(ref _selectedFish, value);
            ((Command)AddFishCommand).ChangeCanExecute();
        }
    }

    public ICommand AddAquariumCommand { get; }
    public ICommand AddFishCommand { get; }

    private bool CanAddAquarium() =>
        !string.IsNullOrWhiteSpace(NewName) && NewVolume > 0;

    private async Task AddAquariumAsync()
    {
        var aquarium = new Aquarium
        {
            Name = NewName,
            VolumeLiters = NewVolume,
            Ph = NewPh,
            Gh = NewGh,
            Kh = NewKh,
            Temperature = NewTemperature
        };

        Aquariums.Add(aquarium);
        await _storageService.AddAquariumAsync(aquarium);

        await RaiseInfoAsync("Aquarium angelegt", $"{aquarium.Name} wurde gespeichert.");

        NewName = string.Empty;
        NewVolume = 60;
        NewPh = 7.0;
        NewGh = 10;
        NewKh = 5;
        NewTemperature = 25.0;
    }

    private async Task AddFishAsync()
    {
        if (SelectedAquarium is null || SelectedFish is null)
        {
            return;
        }

        var validation = ValidateFish(SelectedAquarium, SelectedFish);
        if (validation.Any())
        {
            await RaiseWarningAsync("Hinzufügen nicht möglich", string.Join("\n", validation));
            return;
        }

        SelectedAquarium.Residents.Add(CloneFish(SelectedFish));
        await _storageService.UpdateAquariumAsync();
        await RaiseInfoAsync("Fisch hinzugefügt", $"{SelectedFish.Name} schwimmt jetzt in {SelectedAquarium.Name}.");

        // Trigger UI update
        OnPropertyChanged(nameof(SelectedAquarium));
        SelectedFish = null;
    }

    private List<string> ValidateFish(Aquarium aquarium, Fish fish)
    {
        var errors = new List<string>();

        if (aquarium.VolumeLiters < fish.MinimumTankSizeLiters)
        {
            errors.Add($"Aquarium zu klein: benötigt {fish.MinimumTankSizeLiters} L.");
        }

        if (aquarium.Ph < fish.MinPh || aquarium.Ph > fish.MaxPh)
        {
            errors.Add($"pH-Wert ({aquarium.Ph}) passt nicht zu {fish.MinPh}-{fish.MaxPh}.");
        }

        if (aquarium.Gh < fish.MinGh || aquarium.Gh > fish.MaxGh)
        {
            errors.Add($"Gesamthärte ({aquarium.Gh}) passt nicht zu {fish.MinGh}-{fish.MaxGh}.");
        }

        if (aquarium.Kh < fish.MinKh || aquarium.Kh > fish.MaxKh)
        {
            errors.Add($"Karbonathärte ({aquarium.Kh}) passt nicht zu {fish.MinKh}-{fish.MaxKh}.");
        }

        if (aquarium.Temperature < fish.MinTemperature || aquarium.Temperature > fish.MaxTemperature)
        {
            errors.Add($"Temperatur ({aquarium.Temperature}°C) passt nicht zu {fish.MinTemperature}-{fish.MaxTemperature}°C.");
        }

        foreach (var resident in aquarium.Residents)
        {
            if (fish.Enemies.Any(e => resident.Name.Equals(e, StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add($"{fish.Name} verträgt sich nicht mit {resident.Name}.");
            }

            if (resident.Enemies.Any(e => fish.Name.Equals(e, StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add($"{resident.Name} verträgt sich nicht mit {fish.Name}.");
            }

            if (!string.Equals(resident.Temperament, "friedlich", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fish.Temperament, "friedlich", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{resident.Name} und {fish.Name} sind beide nicht friedlich.");
            }
        }

        return errors;
    }

    /// <summary>
    /// Erstellt eine Kopie des Fisches, damit jedes Aquarium seine eigenen Einträge hält.
    /// </summary>
    private static Fish CloneFish(Fish fish) => new()
    {
        Name = fish.Name,
        MinimumTankSizeLiters = fish.MinimumTankSizeLiters,
        MinPh = fish.MinPh,
        MaxPh = fish.MaxPh,
        MinGh = fish.MinGh,
        MaxGh = fish.MaxGh,
        MinKh = fish.MinKh,
        MaxKh = fish.MaxKh,
        MinTemperature = fish.MinTemperature,
        MaxTemperature = fish.MaxTemperature,
        Origin = fish.Origin,
        Temperament = fish.Temperament,
        Enemies = new List<string>(fish.Enemies)
    };

    private Task RaiseWarningAsync(string title, string message) =>
        WarningRequested?.Invoke(title, message) ?? Task.CompletedTask;

    private Task RaiseInfoAsync(string title, string message) =>
        InfoRequested?.Invoke(title, message) ?? Task.CompletedTask;
}
