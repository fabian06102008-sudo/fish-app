using FishApp.Models;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

[QueryProperty(nameof(Fish), "Fish")]
public partial class FishDetailPage : ContentPage
{
    private Fish? _fish;

    public FishDetailPage()
    {
        InitializeComponent();
    }

    public Fish? Fish
    {
        get => _fish;
        set
        {
            _fish = value;
            BindingContext = value;
        }
    }
}
