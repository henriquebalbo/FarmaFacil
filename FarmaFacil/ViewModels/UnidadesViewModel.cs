using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FarmaFacil.Models;
using FarmaFacil.Services;
using System.Collections.ObjectModel;

namespace FarmaFacil.ViewModels
{
    [QueryProperty(nameof(CodMedicamento), "codMedicamento")]
    [QueryProperty(nameof(NomeMedicamento), "nomeMedicamento")]
    public partial class UnidadesViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private string nomeMedicamento = string.Empty;

        [ObservableProperty]
        private bool carregando = false;

        [ObservableProperty]
        private bool semResultados = false;

        [ObservableProperty]
        private string statusLocalizacao = "Sem localização (ordenação padrão)";

        [ObservableProperty]
        private bool buscandoLocalizacao = false;

        private double _latUsuario = 0;
        private double _lngUsuario = 0;

        private int _codMedicamento;
        public int CodMedicamento
        {
            get => _codMedicamento;
            set
            {
                _codMedicamento = value;
                MainThread.BeginInvokeOnMainThread(async () => await CarregarUnidades());
            }
        }

        public ObservableCollection<EstoqueDetalhado> Unidades { get; } = new();

        public UnidadesViewModel(DatabaseService db)
        {
            _db = db;
        }

        private async Task CarregarUnidades()
        {
            Carregando = true;
            SemResultados = false;
            Unidades.Clear();

            var lista = await _db.BuscarUnidadesPorMedicamentoAsync(_codMedicamento);

            // Se tiver localização do usuário, ordena por distância
            if (_latUsuario != 0 && _lngUsuario != 0)
                lista = OrdenarPorDistancia(lista);

            foreach (var item in lista)
                Unidades.Add(item);

            SemResultados = lista.Count == 0;
            Carregando = false;
        }

        [RelayCommand]
        private async Task UsarLocalizacao()
        {
            try
            {
                BuscandoLocalizacao = true;
                StatusLocalizacao = "Obtendo sua localização...";

                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    StatusLocalizacao = "Permissão de localização negada";
                    BuscandoLocalizacao = false;
                    return;
                }

                var localizacao = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (localizacao != null)
                {
                    _latUsuario = localizacao.Latitude;
                    _lngUsuario = localizacao.Longitude;
                    StatusLocalizacao = $"📍 Ordenado por distância ({_latUsuario:F4}, {_lngUsuario:F4})";
                    await CarregarUnidades();
                }
                else
                {
                    StatusLocalizacao = "Não foi possível obter localização";
                }
            }
            catch (Exception ex)
            {
                StatusLocalizacao = $"Erro: {ex.Message}";
            }
            finally
            {
                BuscandoLocalizacao = false;
            }
        }

        [RelayCommand]
        private async Task AbrirMaps(EstoqueDetalhado item)
        {
            try
            {
                // Extrai coordenadas da unidade
                var coords = item.Coordenadas?.Split(',');
                if (coords != null && coords.Length == 2
                    && double.TryParse(coords[0], System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out double lat)
                    && double.TryParse(coords[1], System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out double lng))
                {
                    // Abre Google Maps com rota até a unidade
                    string url;
                    if (_latUsuario != 0 && _lngUsuario != 0)
                        url = $"https://www.google.com/maps/dir/{_latUsuario},{_lngUsuario}/{lat},{lng}";
                    else
                        url = $"https://www.google.com/maps/search/?api=1&query={lat},{lng}";

                    await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
                else
                {
                    // Busca pelo endereço se não tiver coordenadas
                    var endereco = Uri.EscapeDataString(item.Endereco);
                    var url = $"https://www.google.com/maps/search/?api=1&query={endereco}";
                    await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Erro", $"Não foi possível abrir o Maps: {ex.Message}", "OK");
            }
        }

        private List<EstoqueDetalhado> OrdenarPorDistancia(List<EstoqueDetalhado> lista)
        {
            return lista.OrderBy(item =>
            {
                var coords = item.Coordenadas?.Split(',');
                if (coords != null && coords.Length == 2
                    && double.TryParse(coords[0], System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out double lat)
                    && double.TryParse(coords[1], System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out double lng))
                {
                    return Distancia(_latUsuario, _lngUsuario, lat, lng);
                }
                return double.MaxValue;
            }).ToList();
        }

        // Fórmula de Haversine — distância entre dois pontos GPS
        private static double Distancia(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        [RelayCommand]
        private async Task VerDetalhes(EstoqueDetalhado item)
        {
            await Shell.Current.GoToAsync(
                $"DetalhesPage?codUnidade={item.CodUnidade}&codMedicamento={item.CodMedicamento}");
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}