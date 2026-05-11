using SQLite;

namespace FarmaFacil.Models
{
    [Table("Cidadao")]
    public class Cidadao
    {
        [PrimaryKey, AutoIncrement]
        public int IdCidadao { get; set; }

        [NotNull]
        public string Nome { get; set; } = string.Empty;

        [NotNull, Unique]
        public string Email { get; set; } = string.Empty;

        [NotNull]
        public string Senha { get; set; } = string.Empty;

        public string Endereco { get; set; } = string.Empty;
        public string LocalizacaoAtual { get; set; } = string.Empty;
    }
}