using FarmaFacil.ViewModels;

namespace FarmaFacil.Views
{
    public partial class BuscaPage : ContentPage
    {
        public BuscaPage(BuscaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnimarEntrada();
        }

        private async Task AnimarEntrada()
        {
            this.Opacity = 0;
            await this.FadeToAsync(1, 400, Easing.CubicOut);
        }
    }
}