using System;
using System.Collections.Generic;
using System.Linq;
using FishApp.Models;

namespace FishApp.Services;

/// <summary>
/// Stellt Beispiel-Fische und Suchfunktionalität bereit.
/// </summary>
public class FishRepository
{
    public IList<Fish> FishList { get; } = new List<Fish>
    {
        new()
        {
            Name = "Molly",
            MinimumTankSizeLiters = 80,
            MinPh = 7.0,
            MaxPh = 8.5,
            MinGh = 10,
            MaxGh = 30,
            MinKh = 10,
            MaxKh = 25,
            MinTemperature = 24,
            MaxTemperature = 28,
            Origin = "Zentralamerika",
            Temperament = "friedlich",
            Enemies = new List<string> { "Große Raubfische" }
        },
        new()
        {
            Name = "Betta",
            MinimumTankSizeLiters = 30,
            MinPh = 6.0,
            MaxPh = 7.5,
            MinGh = 3,
            MaxGh = 10,
            MinKh = 2,
            MaxKh = 5,
            MinTemperature = 25,
            MaxTemperature = 30,
            Origin = "Südostasien",
            Temperament = "territorial",
            Enemies = new List<string> { "Andere Betta-Männchen" }
        },
        new()
        {
            Name = "Skalar",
            MinimumTankSizeLiters = 200,
            MinPh = 6.0,
            MaxPh = 7.5,
            MinGh = 3,
            MaxGh = 10,
            MinKh = 2,
            MaxKh = 6,
            MinTemperature = 24,
            MaxTemperature = 30,
            Origin = "Amazonas",
            Temperament = "halb-aggressiv",
            Enemies = new List<string> { "Sehr kleine Fische" }
        },
        new()
        {
            Name = "Oscar",
            MinimumTankSizeLiters = 300,
            MinPh = 6.0,
            MaxPh = 8.0,
            MinGh = 5,
            MaxGh = 20,
            MinKh = 3,
            MaxKh = 15,
            MinTemperature = 23,
            MaxTemperature = 27,
            Origin = "Südamerika",
            Temperament = "aggressiv",
            Enemies = new List<string> { "Kleine Fische" }
        }
    };

    /// <summary>
    /// Liefert einen Fisch anhand des Namens.
    /// </summary>
    public Fish? GetByName(string name) =>
        FishList.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Filtert Fische nach einem Suchbegriff.
    /// </summary>
    public IEnumerable<Fish> Search(string query) =>
        string.IsNullOrWhiteSpace(query)
            ? FishList
            : FishList.Where(f => f.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
}
