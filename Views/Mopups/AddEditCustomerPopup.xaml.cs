using MeuClienteApp.ViewModels;
using Mopups.Pages;
using Mopups.Services;

namespace MeuClienteApp.Views.Mopups;

public partial class AddEditCustomerPopup : PopupPage
{
    public AddEditCustomerPopup(AddEditCustomerViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.RequestClose += async (_, __) =>
        {
            await MopupService.Instance.PopAsync();
        };
    }
}
