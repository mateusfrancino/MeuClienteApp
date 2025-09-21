using Mopups.Pages;
using Mopups.Services;
using Microsoft.Maui.Dispatching;

namespace MeuClienteApp.Views.Mopups;

public partial class ConfirmacaoPopup : PopupPage
{
    private TaskCompletionSource<bool> _tcs = new();

    private readonly bool _focoNoNao;

    public ConfirmacaoPopup(
        string mensagem = "Deseja confirmar?",
        string textoBotaoSim = "Sim",
        string textoBotaoNao = "Não",
        bool focoNoNao = true)
    {
        InitializeComponent();

        MensagemLabel.Text = mensagem;
        SimButton.Text = textoBotaoSim;
        NaoButton.Text = textoBotaoNao;

        _focoNoNao = focoNoNao;
    }

    public Task<bool> GetConfirmationTask() => _tcs.Task;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
        {
            if (_focoNoNao)
                NaoButton.Focus();
            else
                SimButton.Focus();
        });
    }

    private async void OnSimClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(true);
        await MopupService.Instance.PopAsync();
    }

    private async void OnNaoClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(false);
        await MopupService.Instance.PopAsync();
    }

    protected override void OnDisappearing()
    {
        if (!_tcs.Task.IsCompleted)
            _tcs.TrySetResult(false);

        base.OnDisappearing();
    }
}
