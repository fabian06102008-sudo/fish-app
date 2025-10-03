using System.Collections.Generic;

namespace FishApp.Models;

/// <summary>
/// Repräsentiert einen Fisch in der Datenbank.
/// </summary>
public class Fish
{
    public string Name { get; set; } = string.Empty;
    public int MinimumTankSizeLiters { get; set; }
    public double MinPh { get; set; }
    public double MaxPh { get; set; }
    public int MinGh { get; set; }
    public int MaxGh { get; set; }
    public int MinKh { get; set; }
    public int MaxKh { get; set; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Temperament { get; set; } = string.Empty;
    public List<string> Enemies { get; set; } = new();

    public string EnemiesDisplay => Enemies.Count == 0 ? "Keine" : string.Join(", ", Enemies);
}
