using System;
using System.IO;
using System.Text.Json;
using FishApp.Models;
using Microsoft.Maui.Storage;

namespace FishApp.Services;

/// <summary>
/// Verwaltet die lokale JSON-Datei mit Aquariumdaten.
/// </summary>
public class JsonStorageService
{
    private const string FileName = "aquariums.json";

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public IList<Aquarium> Aquariums { get; private set; } = new List<Aquarium>();

    private string GetFilePath()
    {
        // Stelle sicher, dass auch im Testumfeld ein Pfad existiert
        var basePath = FileSystem.Current?.AppDataDirectory ?? Environment.CurrentDirectory;
        return Path.Combine(basePath, FileName);
    }

    /// <summary>
    /// Lädt vorhandene Daten aus der JSON-Datei.
    /// </summary>
    public async Task LoadAsync()
    {
        var path = GetFilePath();
        if (!File.Exists(path))
        {
            Aquariums = new List<Aquarium>();
            await SaveAsync();
            return;
        }

        await using var stream = File.OpenRead(path);
        var data = await JsonSerializer.DeserializeAsync<List<Aquarium>>(stream, _options);
        Aquariums = data ?? new List<Aquarium>();
    }

    /// <summary>
    /// Schreibt die aktuelle Liste in die JSON-Datei.
    /// </summary>
    public async Task SaveAsync()
    {
        var path = GetFilePath();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, Aquariums, _options);
    }

    /// <summary>
    /// Fügt ein neues Aquarium hinzu und speichert sofort.
    /// </summary>
    public async Task AddAquariumAsync(Aquarium aquarium)
    {
        Aquariums.Add(aquarium);
        await SaveAsync();
    }

    /// <summary>
    /// Aktualisiert ein Aquarium (z. B. nach Änderungen an Bewohnern).
    /// </summary>
    public async Task UpdateAquariumAsync()
    {
        await SaveAsync();
    }
}
