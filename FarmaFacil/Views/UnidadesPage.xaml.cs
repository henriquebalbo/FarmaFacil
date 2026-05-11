using FarmaFacil.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace FarmaFacil.Views
{
    public partial class UnidadesPage : ContentPage
    {
        private readonly UnidadesViewModel _viewModel;

        public UnidadesPage(UnidadesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
            viewModel.Unidades.CollectionChanged += Unidades_CollectionChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.Opacity = 0;
            await this.FadeToAsync(1, 350, Easing.CubicOut);
        }

        private void Unidades_CollectionChanged(object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => AtualizarMapa());
        }

        private void AtualizarMapa()
        {
            MapaUnidades.Pins.Clear();

            foreach (var unidade in _viewModel.Unidades)
            {
                var coords = unidade.Coordenadas?.Split(',');
                if (coords != null && coords.Length == 2
                    && double.TryParse(coords[0],
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double lat)
                    && double.TryParse(coords[1],
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double lng))
                {
                    var pin = new Pin
                    {
                        Label = unidade.NomeUnidade,
                        Address = unidade.Endereco,
                        Location = new Location(lat, lng),
                        Type = PinType.Place
                    };
                    MapaUnidades.Pins.Add(pin);
                }
            }

            if (MapaUnidades.Pins.Count > 0)
            {
                var primeiroPin = MapaUnidades.Pins[0];
                MapaUnidades.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        primeiroPin.Location,
                        Distance.FromKilometers(5)));
            }
        }
    }
}