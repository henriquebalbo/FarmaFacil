using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FarmaFacil.Models;
using FarmaFacil.Services;
using System.Collections.ObjectModel;

namespace FarmaFacil.ViewModels
{

    public partial class BuscaViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        private CancellationTokenSource? _cancelToken;

        [ObservableProperty]
        private string termoBusca = string.Empty;

        [ObservableProperty]
        private bool mostrarResultados = false;

        [ObservableProperty]
        private bool mostrarDica = true;

        [ObservableProperty]
        private bool semResultados = false;

        [ObservableProperty]
        private string mensagem = string.Empty;

        public ObservableCollection<Medicamento> Resultados { get; } = new();

        public BuscaViewModel(DatabaseService db)
        {
            _db = db;
        }

        partial void OnTermoBuscaChanged(string value)
        {
            MainThread.BeginInvokeOnMainThread(async () => await BuscarComDelay(value));
        }

        private async Task BuscarComDelay(string valor)
        {
            _cancelToken?.Cancel();
            _cancelToken = new CancellationTokenSource();
            var token = _cancelToken.Token;

            try
            {
                await Task.Delay(300, token);
                if (!token.IsCancellationRequested)
                    await BuscarInterno(valor);
            }
            catch (TaskCanceledException) { }
        }

        private async Task BuscarInterno(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                Resultados.Clear();
                MostrarResultados = false;
                MostrarDica = true;
                SemResultados = false;
                Mensagem = string.Empty;
                return;
            }

            if (valor.Length < 2)
            {
                Mensagem = "Digite pelo menos 2 caracteres...";
                MostrarDica = false;
                MostrarResultados = false;
                SemResultados = false;
                return;
            }

            MostrarDica = false;
            Resultados.Clear();

            var lista = await _db.BuscarMedicamentoAsync(valor);

            foreach (var item in lista)
                Resultados.Add(item);

            Mensagem = lista.Count > 0
                ? $"{lista.Count} medicamento(s) encontrado(s)"
                : string.Empty;

            SemResultados = lista.Count == 0;
            MostrarResultados = lista.Count > 0;
        }

        [RelayCommand]
        private async Task VerOnde(Medicamento medicamento)
        {
            await Shell.Current.GoToAsync(
                $"UnidadesPage?codMedicamento={medicamento.CodMedicamento}&nomeMedicamento={medicamento.Nome}");
        }

        [RelayCommand]
        private async Task BuscarExemplo(string termo)
        {
            TermoBusca = termo;
        }

        public void LimparBusca()
        {
            TermoBusca = string.Empty;
            Resultados.Clear();
            MostrarResultados = false;
            MostrarDica = true;
            SemResultados = false;
            Mensagem = string.Empty;
        }
    }
}