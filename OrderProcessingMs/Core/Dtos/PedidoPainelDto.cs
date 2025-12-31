using Core.Enums;

namespace Core.Dtos
{
    public class PedidoPainelDto
    {
        public int IdPedido { get; set; }
        public StatusPedidoEnum? StatusPedidoEnum { get; set; }
        public StatusPagamentoEnum? StatusPagamentoEnum { get; set; }
        public DateTime? DataHoraInicioPedido { get; set; }
        public DateTime? DataHoraPagamento { get; set; }
        public DateTime? DataHoraInicioPreparo { get; set; }
        public DateTime? DataHoraTerminoPreparo { get; set; }
        public DateTime? DataHoraRetiradaCliente { get; set; }
        public List<FullComboDto> Combos { get; set; }
    }
}
