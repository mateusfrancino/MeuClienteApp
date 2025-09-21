using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeuClienteApp.Helpers;
using MeuClienteApp.Models;
using MeuClienteApp.Services;
using MeuClienteApp.Views.Mopups;
using Mopups.Services;

namespace MeuClienteApp.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    private readonly ICustomerService _service;
    private readonly IServiceProvider _sp;

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty]
    private Customer? selectedCustomer;

    public HomePageViewModel(ICustomerService service, IServiceProvider sp)
    {
        _service = service;
        _sp = sp;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var list = await _service.GetAllAsync();
        Customers.Clear();
        foreach (var c in list)
            Customers.Add(c);

        SelectedCustomer = Customers.FirstOrDefault();
    }

    // INCLUIR 
    [RelayCommand]
    public async Task NewAsync()
    {
        var popup = _sp.GetRequiredService<AddEditCustomerPopup>();
        var vm = (AddEditCustomerViewModel)popup.BindingContext;

        vm.InitializeForNew();

        await MopupService.Instance.PushAsync(popup);
        var result = await vm.ResultTask;

        if (result is null) return;

        Customers.Add(result);
        SelectedCustomer = result;
        await _service.AddOrUpdateAsync(result);
    }

    // ALTERAR 
    [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
    public async Task EditAsync()
    {
        if (SelectedCustomer is null) return;

        var popup = _sp.GetRequiredService<AddEditCustomerPopup>();
        var vm = (AddEditCustomerViewModel)popup.BindingContext;

        vm.InitializeForEdit(SelectedCustomer);

        await MopupService.Instance.PushAsync(popup);
        var result = await vm.ResultTask;

        if (result is null) return;

        var idx = Customers.IndexOf(SelectedCustomer);
        if (idx >= 0) Customers[idx] = result;
        SelectedCustomer = result;

        await _service.AddOrUpdateAsync(result);
    }

    // EXCLUIR
    [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
    public async Task DeleteAsync()
    {
        if (SelectedCustomer is null) return;

        bool confirmado = await FuncoesBasicas.ExibirPopupConfirmacaoSimNao(
                        $"Deseja realmente excluir o cliente \"{SelectedCustomer.Name}\"?",
                        textoBotaoSim: "Excluir",
                        textoBotaoNao: "Cancelar",
                        focoNoNao: true);

        if (!confirmado)
            return;

        var id = SelectedCustomer.Id;
        Customers.Remove(SelectedCustomer);
        SelectedCustomer = Customers.FirstOrDefault();
        await _service.DeleteAsync(id);
    }

    private bool CanEditOrDelete() => SelectedCustomer is not null;

    partial void OnSelectedCustomerChanged(Customer? value)
    {
        EditCommand.NotifyCanExecuteChanged();
        DeleteCommand.NotifyCanExecuteChanged();
    }
}
