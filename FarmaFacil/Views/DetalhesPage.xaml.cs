using FarmaFacil.ViewModels;

namespace FarmaFacil.Views
{
    public partial class DetalhesPage : ContentPage
    {
        public DetalhesPage(DetalhesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            await this.FadeToAsync(1, 350, Easing.CubicOut);
        }
    }
}