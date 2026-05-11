using FarmaFacil.Services;
using FarmaFacil.Views;

namespace FarmaFacil
{
    public partial class App : Application
    {
        public App(DatabaseService db)
        {
            InitializeComponent();
            Task.Run(async () => await db.InitAsync()).Wait();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = IPlatformApplication.Current!.Services.GetRequiredService<AppShell>();
            return new Window(shell);
        }
    }
}