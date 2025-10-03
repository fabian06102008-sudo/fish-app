using System.Collections.Generic;

namespace FishApp.Models;

/// <summary>
/// Aquarium mit Nutzerdaten und eingesetzten Fischen.
/// </summary>
public class Aquarium
{
    public string Name { get; set; } = string.Empty;
    public int VolumeLiters { get; set; }
    public double Ph { get; set; } = 7.0;
    public int Gh { get; set; } = 10;
    public int Kh { get; set; } = 5;
    public double Temperature { get; set; } = 25.0;
    public List<Fish> Residents { get; set; } = new();

    public override string ToString() => $"{Name} ({VolumeLiters} L)";
}
