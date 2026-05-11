namespace FarmaFacil.Models
{
    public class EstoqueDetalhado
    {
        public string NomeMedicamento { get; set; } = string.Empty;
        public string PrincipioAtivo { get; set; } = string.Empty;
        public string Dosagem { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string NomeUnidade { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;
        public string Coordenadas { get; set; } = string.Empty;
        public int QuantidadeDisponivel { get; set; }
        public int CodUnidade { get; set; }
        public int CodMedicamento { get; set; }

        public string CorEstoque => QuantidadeDisponivel > 100 ? "#2ecc71" : "#e67e22";
    }
}