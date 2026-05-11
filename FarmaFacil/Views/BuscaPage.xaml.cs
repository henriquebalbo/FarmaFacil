using FarmaFacil.ViewModels;

namespace FarmaFacil.Views
{
    public partial class BuscaPage : ContentPage
    {
        private readonly BuscaViewModel _viewModel;

        public BuscaPage(BuscaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            await this.FadeToAsync(1, 400, Easing.CubicOut);
            _viewModel.LimparBusca();
            await Task.Delay(500);
            EntryBusca.Focus();
        }
    }
}