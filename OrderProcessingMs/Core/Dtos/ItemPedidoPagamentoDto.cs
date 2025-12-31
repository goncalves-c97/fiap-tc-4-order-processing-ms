namespace Core.Dtos
{
    public class ItemPedidoPagamentoDto
    {
        public string Id { get; set; }
        public string Categoria { get; set; }
        public string? Descricao { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get => PrecoUnitario * Quantidade; }
    }
}
