using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FarmaFacil.Models;
using FarmaFacil.Services;
using System.Collections.ObjectModel;

namespace FarmaFacil.ViewModels
{
    [QueryProperty(nameof(Termo), "termo")]
    public partial class ResultadoViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        private CancellationTokenSource? _cancelToken;

        [ObservableProperty]
        private string termo = string.Empty;

        [ObservableProperty]
        private bool carregando = false;

        [ObservableProperty]
        private bool semResultados = false;

        [ObservableProperty]
        private string mensagemResultado = string.Empty;

        public ObservableCollection<Medicamento> Resultados { get; } = new();

        public ResultadoViewModel(DatabaseService db)
        {
            _db = db;
        }

        // Disparado automaticamente a cada letra digitada
        partial void OnTermoChanged(string value)
        {
            MainThread.BeginInvokeOnMainThread(async () => await BuscarComDelay(value));
        }

        // Aguarda 300ms após parar de digitar para buscar (debounce)
        private async Task BuscarComDelay(string valor)
        {
            _cancelToken?.Cancel();
            _cancelToken = new CancellationTokenSource();
            var token = _cancelToken.Token;

            try
            {
                await Task.Delay(300, token);

                if (!token.IsCancellationRequested)
                    await Buscar();
            }
            catch (TaskCanceledException)
            {
                // Digitação ainda em andamento — ignora
            }
        }

        [RelayCommand]
        private async Task Buscar()
        {
            if (string.IsNullOrWhiteSpace(Termo))
            {
                Resultados.Clear();
                MensagemResultado = string.Empty;
                SemResultados = false;
                return;
            }

            if (Termo.Length < 2)
            {
                MensagemResultado = "Digite pelo menos 2 caracteres...";
                SemResultados = false;
                return;
            }

            Carregando = true;
            SemResultados = false;
            Resultados.Clear();

            var lista = await _db.BuscarMedicamentoAsync(Termo);

            foreach (var item in lista)
                Resultados.Add(item);

            MensagemResultado = lista.Count > 0
                ? $"{lista.Count} medicamento(s) encontrado(s)"
                : string.Empty;

            SemResultados = lista.Count == 0;
            Carregando = false;
        }

        [RelayCommand]
        private async Task VerOnde(Medicamento medicamento)
        {
            await Shell.Current.GoToAsync(
                $"UnidadesPage?codMedicamento={medicamento.CodMedicamento}&nomeMedicamento={medicamento.Nome}");
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}