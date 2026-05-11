using FarmaFacil.Services;
using FarmaFacil.ViewModels;
using FarmaFacil.Views;
using Microsoft.Extensions.Logging;

namespace FarmaFacil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                  .UseMauiApp<App>()
                  .UseMauiMaps()
                  .ConfigureFonts(fonts =>
                  {
                      fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                      fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                  });

            // Serviços
            builder.Services.AddSingleton<DatabaseService>();

            // ViewModels
            builder.Services.AddSingleton<BuscaViewModel>();
            builder.Services.AddTransient<ResultadoViewModel>();
            builder.Services.AddTransient<UnidadesViewModel>();
            builder.Services.AddTransient<DetalhesViewModel>();

            // Páginas
            builder.Services.AddSingleton<BuscaPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddTransient<ResultadoPage>();
            builder.Services.AddTransient<UnidadesPage>();
            builder.Services.AddTransient<DetalhesPage>();

            // Shell
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}