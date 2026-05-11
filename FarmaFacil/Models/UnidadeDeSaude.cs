using SQLite;

namespace FarmaFacil.Models
{
    [Table("Unidade_de_Saude")]
    public class UnidadeDeSaude
    {
        [PrimaryKey, AutoIncrement]
        public int CodUnidade { get; set; }

        [NotNull]
        public string Nome { get; set; } = string.Empty;

        [NotNull]
        public string Endereco { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;
        public string HorarioFuncionamento { get; set; } = string.Empty;
        public string Coordenadas { get; set; } = string.Empty;
    }
}