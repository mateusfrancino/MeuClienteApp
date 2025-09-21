using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeuClienteApp.Models;
using System.Net;
using System.Xml.Linq;

namespace MeuClienteApp.ViewModels;

public partial class AddEditCustomerViewModel : ObservableObject
{
    private readonly TaskCompletionSource<Customer?> _resultTcs = new();
    public Task<Customer?> ResultTask => _resultTcs.Task;

    [ObservableProperty] private string title = "Novo Cliente";
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string lastname = string.Empty;
    [ObservableProperty] private string ageText = string.Empty; 
    [ObservableProperty] private string address = string.Empty;

    private Guid _editingId = Guid.Empty;

    public void InitializeForNew()
    {
        Title = "Novo Cliente";
        _editingId = Guid.Empty;
        Name = Lastname = Address = string.Empty;
        AgeText = string.Empty;
    }

    public void InitializeForEdit(Customer c)
    {
        Title = "Editar Cliente";
        _editingId = c.Id;
        Name = c.Name;
        Lastname = c.Lastname;
        AgeText = c.Age.ToString();
        Address = c.Address;
    }

    public event EventHandler<Customer?>? RequestClose;

    [RelayCommand]
    private void Cancel() => Close(null);

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Lastname))
        {
            return;
        }

        if (!int.TryParse(AgeText, out var age) || age < 0 || age > 130)
            age = 0;

        var id = _editingId == Guid.Empty ? Guid.NewGuid() : _editingId;
        var result = new Customer
        {
            Id = id,
            Name = Name.Trim(),
            Lastname = Lastname.Trim(),
            Age = age,
            Address = Address?.Trim() ?? string.Empty
        };

        Close(result);
    }

    private void Close(Customer? result)
    {
        if (!_resultTcs.Task.IsCompleted)
            _resultTcs.TrySetResult(result);

        RequestClose?.Invoke(this, result);
    }
}
