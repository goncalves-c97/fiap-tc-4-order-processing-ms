using Core.Interfaces.Gateways;
using Core.Interfaces;
using Core.Entities;
using Core.Dtos;

namespace Core.Gateways
{
    public class ComboGateway(IDbConnection dbConnection) : IComboGateway
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string TableName = nameof(Combo);

        public async Task<Combo?> GetById(int idCombo)
        {
            return await _dbConnection.SearchFirstOrDefaultByParametersAsync<Combo>(
                TableName,
                "id_combo = @Id",
                new { Id = idCombo }
            );
        }

        public async Task<Combo?> GetCompleteComboById(int idCombo)
        {
            var sql = @"
                SELECT 
                    c.*, 
                    l.id_lanche, l.Nome, l.Preco,
                    a.id_acompanhamento, a.Nome, a.Preco,
                    b.id_bebida, b.Nome, b.Preco,
                    s.id_sobremesa, s.Nome, s.Preco
                FROM Combo c
                LEFT JOIN Lanche l ON c.id_lanche = l.id_lanche
                LEFT JOIN Acompanhamento a ON c.id_acompanhamento = a.id_acompanhamento
                LEFT JOIN Bebida b ON c.id_bebida = b.id_bebida
                LEFT JOIN Sobremesa s ON c.id_sobremesa = s.id_sobremesa
                WHERE c.id_combo = @Id
            ";

            Combo combo = (await ExecuteRawSql<Combo, Lanche, Acompanhamento, Bebida, Sobremesa, Combo>(
                sql,
                (combo, lanche, acompanhamento, bebida, sobremesa) =>
                {
                    combo.IdLancheNavigation = lanche;
                    combo.IdAcompanhamentoNavigation = acompanhamento;
                    combo.IdBebidaNavigation = bebida;
                    combo.IdSobremesaNavigation = sobremesa;
                    return combo;
                },
                new { Id = idCombo },
                splitOn: "id_lanche,id_acompanhamento,id_bebida,id_sobremesa"
            )).FirstOrDefault()!;

            if (combo.IdLancheNavigation != null)
            {
                combo.IdLancheNavigation!.IngredientesLanche = (await _dbConnection.SearchByParametersAsync<IngredientesLanche>(
                    "Ingredientes_lanche",
                    "id_lanche = @Id",
                    new { Id = combo.IdLanche }
                )).ToList();

                foreach (IngredientesLanche ingrediente in combo.IdLancheNavigation.IngredientesLanche)
                {
                    ingrediente.IdIngredienteNavigation = (await _dbConnection.SearchFirstOrDefaultByParametersAsync<Ingrediente>(
                        "Ingrediente",
                        "id_ingrediente = @Id",
                        new { Id = ingrediente.IdIngrediente }
                    ))!;
                }

                combo.AlteracoesIngredientes = (await _dbConnection.SearchByParametersAsync<AlteracaoIngredienteCombo>(
                    "Alteracao_ingrediente_combo",
                    "id_combo = @Id",
                    new { Id = idCombo }
                )).ToList();

                foreach (AlteracaoIngredienteCombo alteracao in combo.AlteracoesIngredientes)
                {
                    alteracao.IdIngredienteNavigation = (await _dbConnection.SearchFirstOrDefaultByParametersAsync<Ingrediente>(
                        "Ingrediente",
                        "id_ingrediente = @Id",
                        new { Id = alteracao.IdIngrediente }
                    ))!;
                }
            }

            return combo;
        }

        public async Task<Combo> Insert(ComboDto comboDto)
        {
            int registeredId = await _dbConnection.InsertAndReturnIdAsync(TableName, new Dictionary<string, object>
            {
                { "id_lanche", comboDto.IdLanche },
                { "id_acompanhamento", comboDto.IdAcompanhamento },
                { "id_bebida", comboDto.IdBebida },
                { "id_sobremesa", comboDto.IdSobremesa },

            }, "id_combo");

            return await GetById(registeredId) ?? throw new Exception("Insert failed");
        }

        public async Task<IEnumerable<TReturn>> ExecuteRawSql<T1, T2, T3, T4, T5, TReturn>(
        string sql,
        Func<T1, T2, T3, T4, T5, TReturn> map,
        object? param = null,
        string splitOn = "Id"
    )
        {
            return await _dbConnection.QueryAsync(sql, map, param, splitOn: splitOn);
        }
    }
}