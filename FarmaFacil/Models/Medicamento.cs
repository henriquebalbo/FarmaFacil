using SQLite;

namespace FarmaFacil.Models
{
    [Table("Medicamentos")]
    public class Medicamento
    {
        [PrimaryKey, AutoIncrement]
        public int CodMedicamento { get; set; }

        [NotNull]
        public string Nome { get; set; } = string.Empty;

        public string PrincipioAtivo { get; set; } = string.Empty;
        public string Dosagem { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
    }
}