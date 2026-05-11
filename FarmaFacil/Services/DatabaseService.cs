using SQLite;
using FarmaFacil.Models;

namespace FarmaFacil.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database = null!;
        private static DatabaseService? _instance;

        public static DatabaseService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseService();
                return _instance;
            }
        }

        // Caminho do banco de dados no dispositivo
        private static string DbPath =>
            Path.Combine(FileSystem.AppDataDirectory, "farmafacil.db3");

        // Inicializa o banco e cria as tabelas se não existirem
        public async Task InitAsync()
        {
            if (_database != null)
                return;

            // Apaga o banco antigo para recriar com estrutura correta
            if (File.Exists(DbPath))
                File.Delete(DbPath);

            _database = new SQLiteAsyncConnection(DbPath);

            await _database.CreateTableAsync<Cidadao>();
            await _database.CreateTableAsync<UnidadeDeSaude>();
            await _database.CreateTableAsync<Medicamento>();
            await _database.CreateTableAsync<Estoque>();

            await PopularDadosIniciaisAsync();
        }

        // ─────────────────────────────────────────
        // POPULAR DADOS DE TESTE (simula o HÓRUS)
        // ─────────────────────────────────────────
        private async Task PopularDadosIniciaisAsync()
        {
            // Só popula se o banco estiver vazio
            var count = await _database.Table<Medicamento>().CountAsync();
            if (count > 0) return;

            // Medicamentos
            var medicamentos = new List<Medicamento>
            {
                new Medicamento { Nome = "Dipirona", PrincipioAtivo = "Dipirona Sódica", Dosagem = "500mg", Fabricante = "EMS" },
                new Medicamento { Nome = "Paracetamol", PrincipioAtivo = "Paracetamol", Dosagem = "750mg", Fabricante = "Medley" },
                new Medicamento { Nome = "Ibuprofeno", PrincipioAtivo = "Ibuprofeno", Dosagem = "600mg", Fabricante = "Neo Química" },
                new Medicamento { Nome = "Losartana", PrincipioAtivo = "Losartana Potássica", Dosagem = "50mg", Fabricante = "EMS" },
                new Medicamento { Nome = "Amoxicilina", PrincipioAtivo = "Amoxicilina Tri-hidratada", Dosagem = "500mg", Fabricante = "Teuto" },
                new Medicamento { Nome = "Metformina", PrincipioAtivo = "Cloridrato de Metformina", Dosagem = "850mg", Fabricante = "Medley" },
                new Medicamento { Nome = "Atenolol", PrincipioAtivo = "Atenolol", Dosagem = "25mg", Fabricante = "Neo Química" },
                new Medicamento { Nome = "Omeprazol", PrincipioAtivo = "Omeprazol", Dosagem = "20mg", Fabricante = "EMS" },
            };

            await _database.InsertAllAsync(medicamentos);

            // Unidades de Saúde
            var unidades = new List<UnidadeDeSaude>
            {
                new UnidadeDeSaude
                {
                    Nome = "UPA 24h Norte",
                    Endereco = "Av. Brasil, 500 - Jd. América",
                    Telefone = "(17) 3842-2000",
                    HorarioFuncionamento = "24 horas",
                    Coordenadas = "-20.8113,-49.3758"
                },
                new UnidadeDeSaude
                {
                    Nome = "UBS Jardim Europa",
                    Endereco = "Rua Paraná, 80 - Jd. Europa",
                    Telefone = "(17) 3842-3100",
                    HorarioFuncionamento = "Seg-Sex 07:00 às 17:00",
                    Coordenadas = "-20.8200,-49.3800"
                },
                new UnidadeDeSaude
                {
                    Nome = "Farmácia Popular Centro",
                    Endereco = "Rua XV de Novembro, 210 - Centro",
                    Telefone = "(17) 3842-4000",
                    HorarioFuncionamento = "Seg-Sex 08:00 às 18:00 | Sáb 08:00 às 12:00",
                    Coordenadas = "-20.8050,-49.3700"
                },
                new UnidadeDeSaude
                {
                    Nome = "UBS Vila Nova",
                    Endereco = "Rua das Flores, 320 - Vila Nova",
                    Telefone = "(17) 3842-5000",
                    HorarioFuncionamento = "Seg-Sex 07:00 às 16:00",
                    Coordenadas = "-20.8300,-49.3900"
                },
            };

            await _database.InsertAllAsync(unidades);

            // Estoque — liga medicamentos às unidades
            var estoques = new List<Estoque>
            {
                // UPA 24h Norte (cod 1)
                new Estoque { CodUnidade = 1, CodMedicamento = 1, QuantidadeDisponivel = 500 },
                new Estoque { CodUnidade = 1, CodMedicamento = 2, QuantidadeDisponivel = 300 },
                new Estoque { CodUnidade = 1, CodMedicamento = 4, QuantidadeDisponivel = 150 },
                new Estoque { CodUnidade = 1, CodMedicamento = 8, QuantidadeDisponivel = 200 },

                // UBS Jardim Europa (cod 2)
                new Estoque { CodUnidade = 2, CodMedicamento = 1, QuantidadeDisponivel = 220 },
                new Estoque { CodUnidade = 2, CodMedicamento = 3, QuantidadeDisponivel = 180 },
                new Estoque { CodUnidade = 2, CodMedicamento = 5, QuantidadeDisponivel = 90  },
                new Estoque { CodUnidade = 2, CodMedicamento = 6, QuantidadeDisponivel = 400 },

                // Farmácia Popular Centro (cod 3)
                new Estoque { CodUnidade = 3, CodMedicamento = 2, QuantidadeDisponivel = 50  },
                new Estoque { CodUnidade = 3, CodMedicamento = 4, QuantidadeDisponivel = 310 },
                new Estoque { CodUnidade = 3, CodMedicamento = 6, QuantidadeDisponivel = 120 },
                new Estoque { CodUnidade = 3, CodMedicamento = 7, QuantidadeDisponivel = 75  },

                // UBS Vila Nova (cod 4)
                new Estoque { CodUnidade = 4, CodMedicamento = 3, QuantidadeDisponivel = 260 },
                new Estoque { CodUnidade = 4, CodMedicamento = 5, QuantidadeDisponivel = 140 },
                new Estoque { CodUnidade = 4, CodMedicamento = 7, QuantidadeDisponivel = 95  },
                new Estoque { CodUnidade = 4, CodMedicamento = 8, QuantidadeDisponivel = 330 },
            };

            await _database.InsertAllAsync(estoques);
        }

        // ─────────────────────────────────────────
        // CONSULTAS
        // ─────────────────────────────────────────

        // Busca medicamentos pelo nome (busca parcial)
        public async Task<List<Medicamento>> BuscarMedicamentoAsync(string nome)
        {
            var todos = await _database.Table<Medicamento>().ToListAsync();

            return todos
                .Where(m =>
                    m.Nome.StartsWith(nome, StringComparison.OrdinalIgnoreCase) ||
                    m.PrincipioAtivo.StartsWith(nome, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Busca unidades que têm o medicamento em estoque
        public async Task<List<EstoqueDetalhado>> BuscarUnidadesPorMedicamentoAsync(int codMedicamento)
        {
            var estoques = await _database.Table<Estoque>()
                .Where(e => e.CodMedicamento == codMedicamento)
                .ToListAsync();

            var medicamento = await _database.Table<Medicamento>()
                .Where(m => m.CodMedicamento == codMedicamento)
                .FirstOrDefaultAsync();

            var resultado = new List<EstoqueDetalhado>();

            foreach (var estoque in estoques)
            {
                var unidade = await _database.Table<UnidadeDeSaude>()
                    .Where(u => u.CodUnidade == estoque.CodUnidade)
                    .FirstOrDefaultAsync();

                if (unidade != null && medicamento != null)
                {
                    resultado.Add(new EstoqueDetalhado
                    {
                        CodUnidade = unidade.CodUnidade,
                        CodMedicamento = medicamento.CodMedicamento,
                        NomeMedicamento = medicamento.Nome,
                        PrincipioAtivo = medicamento.PrincipioAtivo,
                        Dosagem = medicamento.Dosagem,
                        Fabricante = medicamento.Fabricante,
                        NomeUnidade = unidade.Nome,
                        Endereco = unidade.Endereco,
                        Telefone = unidade.Telefone,
                        HorarioFuncionamento = unidade.HorarioFuncionamento,
                        Coordenadas = unidade.Coordenadas,
                        QuantidadeDisponivel = estoque.QuantidadeDisponivel
                    });
                }
            }

            return resultado;
        }

        // Busca detalhes completos de uma unidade específica
        public async Task<UnidadeDeSaude> BuscarDetalhesUnidadeAsync(int codUnidade)
        {
            return await _database.Table<UnidadeDeSaude>()
                .Where(u => u.CodUnidade == codUnidade)
                .FirstOrDefaultAsync();
        }
    }
}