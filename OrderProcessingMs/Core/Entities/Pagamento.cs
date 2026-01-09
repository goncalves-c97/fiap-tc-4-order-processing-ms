namespace Core.Entities;

public partial class Pagamento
{
    public int IdPagamento { get; set; }

    public int IdPedido { get; set; }

    public int IdGatewayPagamento { get; set; }

    public decimal Valor { get; set; }

    public int IdStatusPagamento { get; set; }

    public DateTime? DataHoraPago { get; set; }
}
