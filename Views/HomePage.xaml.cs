using MeuClienteApp.ViewModels;

namespace MeuClienteApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _vm;
    private bool _loaded;

    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded) return;        
        _loaded = true;

        await _vm.LoadAsync();
    }
}
