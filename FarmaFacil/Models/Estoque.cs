using SQLite;

namespace FarmaFacil.Models
{
    [Table("Estoque")]
    public class Estoque
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int CodUnidade { get; set; }

        [Indexed]
        public int CodMedicamento { get; set; }

        [NotNull]
        public int QuantidadeDisponivel { get; set; }
    }
}