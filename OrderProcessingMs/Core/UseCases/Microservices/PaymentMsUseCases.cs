using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways.Microservices;

namespace Core.UseCases.Microservices
{
    public static class PaymentMsUseCases
    {
        /// <summary>
        /// Realiza o checkout do pedido na peça de pagamento e retorna o QR Code para pagamento
        /// </summary>
        /// <returns></returns>
        public static async Task<QrCodePagamentoDto> CheckoutPedido(IPaymentMsGateway paymentMsGateway, int idPedido, IEnumerable<Combo> combos, string token)
        {
            List<ItemPedidoPagamentoDto> itens = [];

            foreach (Combo combo in combos)
            {
                itens.AddRange(GetItemPedidoByCombo(combo));
            }

            int valorTotalPedidoCentavos = (int)itens
                    .Select(x => x.ValorTotal)
                    .Sum()
                    * 100; // Converte p/ centavos, para trabalhar com inteiros

            return await paymentMsGateway.CheckoutPedido(idPedido, valorTotalPedidoCentavos, token);
        }

        private static List<ItemPedidoPagamentoDto> GetItemPedidoByCombo(Combo combo)
        {
            List<ItemPedidoPagamentoDto> itensPedidoPagamento = [];

            if (combo.IdLanche != null)
            {
                ItemPedidoPagamentoDto itemPedidoPagamentoDto = new ItemPedidoPagamentoDto
                {
                    Id = combo.IdLanche.ToString(),
                    Categoria = "Lanche",
                    Descricao = combo.IdLancheNavigation.Nome,
                    PrecoUnitario = combo.IdLancheNavigation.Preco,
                    Quantidade = 1
                };

                if (combo.AlteracoesIngredientes.Count > 0)
                {
                    foreach (AlteracaoIngredienteCombo alteracao in combo.AlteracoesIngredientes)
                    {
                        itemPedidoPagamentoDto.Descricao += $" (*** {alteracao.IdIngredienteNavigation.Nome} x{alteracao.Quantidade})";
                        IngredientesLanche? ingredienteOriginal = combo.IdLancheNavigation.IngredientesLanche.FirstOrDefault(x => x.IdIngrediente == alteracao.IdIngrediente);

                        if (ingredienteOriginal != null)
                        {
                            if (alteracao.Quantidade > ingredienteOriginal.Quantidade)
                                itemPedidoPagamentoDto.PrecoUnitario += (decimal)((alteracao.Quantidade - ingredienteOriginal.Quantidade) * ingredienteOriginal.IdIngredienteNavigation.PrecoAdicional!);
                        }
                        else
                            itemPedidoPagamentoDto.PrecoUnitario += (decimal)(alteracao.Quantidade * alteracao.IdIngredienteNavigation.PrecoAdicional);
                    }

                }
                itensPedidoPagamento.Add(itemPedidoPagamentoDto);
            }

            if (combo.IdAcompanhamento != null)
                itensPedidoPagamento.Add(GetItemPedidoPagamentoDtoByComboItem((int)combo.IdAcompanhamento, combo.IdAcompanhamentoNavigation!));

            if (combo.IdBebida != null)
                itensPedidoPagamento.Add(GetItemPedidoPagamentoDtoByComboItem((int)combo.IdBebida, combo.IdBebidaNavigation!));

            if (combo.IdSobremesa != null)
                itensPedidoPagamento.Add(GetItemPedidoPagamentoDtoByComboItem((int)combo.IdSobremesa, combo.IdSobremesaNavigation!));

            return itensPedidoPagamento;
        }

        private static ItemPedidoPagamentoDto GetItemPedidoPagamentoDtoByComboItem(int id, dynamic itemCombo)
        {
            ItemPedidoPagamentoDto itemPedidoPagamentoDto = new ItemPedidoPagamentoDto
            {
                Id = id.ToString(),
                Categoria = itemCombo.GetType().Name,
                Descricao = itemCombo.Nome,
                PrecoUnitario = itemCombo.Preco,
                Quantidade = 1
            };

            return itemPedidoPagamentoDto;
        }
    }
}
