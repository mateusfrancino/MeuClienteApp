using MeuClienteApp.Views.Mopups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuClienteApp.Helpers
{
    public static class FuncoesBasicas
    {
        public static async Task<bool> ExibirPopupConfirmacaoSimNao(string mensagem, string textoBotaoSim = "Sim", string textoBotaoNao = "Não", bool focoNoNao = true)
        {
            var popup = new ConfirmacaoPopup(mensagem, focoNoNao: focoNoNao, textoBotaoNao: textoBotaoNao, textoBotaoSim: textoBotaoSim);

            await MopupService.Instance.PushAsync(popup);

            return await popup.GetConfirmationTask();
        }
    }
}
