using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;      
using Mopups.Hosting;              
using MeuClienteApp.Services;
using MeuClienteApp.ViewModels;
using MeuClienteApp.Views;
using MeuClienteApp.Views.Mopups;

namespace MeuClienteApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()   
                .ConfigureMopups()           
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<AddEditCustomerViewModel>();
            builder.Services.AddTransient<AddEditCustomerPopup>();

            return builder.Build();
        }
    }
}
