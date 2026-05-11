using FarmaFacil.ViewModels;

namespace FarmaFacil.Views
{
    public partial class ResultadoPage : ContentPage
    {
        public ResultadoPage(ResultadoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}