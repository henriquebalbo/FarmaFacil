using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FarmaFacil.Models;
using FarmaFacil.Services;

namespace FarmaFacil.ViewModels
{
    [QueryProperty(nameof(CodUnidade), "codUnidade")]
    [QueryProperty(nameof(CodMedicamento), "codMedicamento")]
    public partial class DetalhesViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private string nomeUnidade = string.Empty;

        [ObservableProperty]
        private string endereco = string.Empty;

        [ObservableProperty]
        private string telefone = string.Empty;

        [ObservableProperty]
        private string horario = string.Empty;

        [ObservableProperty]
        private string nomeMedicamento = string.Empty;

        [ObservableProperty]
        private string dosagem = string.Empty;

        [ObservableProperty]
        private string fabricante = string.Empty;

        [ObservableProperty]
        private int quantidadeDisponivel = 0;

        [ObservableProperty]
        private string corEstoque = "#2ecc71";

        private int _codUnidade;
        public int CodUnidade
        {
            get => _codUnidade;
            set
            {
                _codUnidade = value;
                if (_codMedicamento > 0)
                    MainThread.BeginInvokeOnMainThread(async () => await CarregarDetalhes());
            }
        }

        private int _codMedicamento;
        public int CodMedicamento
        {
            get => _codMedicamento;
            set
            {
                _codMedicamento = value;
                if (_codUnidade > 0)
                    MainThread.BeginInvokeOnMainThread(async () => await CarregarDetalhes());
            }
        }

        public DetalhesViewModel(DatabaseService db)
        {
            _db = db;
        }

        private async Task CarregarDetalhes()
        {
            var lista = await _db.BuscarUnidadesPorMedicamentoAsync(_codMedicamento);
            var item = lista.FirstOrDefault(e => e.CodUnidade == _codUnidade);

            if (item != null)
            {
                NomeUnidade = item.NomeUnidade;
                Endereco = item.Endereco;
                Telefone = item.Telefone;
                Horario = item.HorarioFuncionamento;
                NomeMedicamento = item.NomeMedicamento;
                Dosagem = item.Dosagem;
                Fabricante = item.Fabricante;
                QuantidadeDisponivel = item.QuantidadeDisponivel;
                CorEstoque = item.CorEstoque;
            }
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task VoltarInicio()
        {
            await Shell.Current.GoToAsync("//BuscaPage");
        }

        [RelayCommand]
        private void Ligar()
        {
            if (!string.IsNullOrEmpty(Telefone))
                PhoneDialer.Default.Open(Telefone);
        }
    }
}