using FarmaFacil.Views;

namespace FarmaFacil
{
    public partial class AppShell : Shell
    {
        public AppShell(BuscaPage buscaPage, AboutPage aboutPage)
        {
            InitializeComponent();

            var tabBar = new TabBar();

            tabBar.Items.Add(new ShellContent
            {
                Title = "Buscar",
                Route = "BuscaPage",
                Content = buscaPage
            });

            tabBar.Items.Add(new ShellContent
            {
                Title = "Sobre",
                Route = "AboutPage",
                Content = aboutPage
            });

            Items.Add(tabBar);

            Routing.RegisterRoute(nameof(ResultadoPage), typeof(ResultadoPage));
            Routing.RegisterRoute(nameof(UnidadesPage), typeof(UnidadesPage));
            Routing.RegisterRoute(nameof(DetalhesPage), typeof(DetalhesPage));
        }
    }
}