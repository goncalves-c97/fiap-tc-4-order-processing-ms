using Core.Interfaces.Gateways;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IAcompanhamentoGateway AcompanhamentoRepository { get; }
        public IAlteracaoIngredienteComboGateway AlteracaoIngredienteComboRepository { get; }
        public IBebidaGateway BebidaRepository { get; }
        public IComboGateway ComboRepository { get; }
        public IIngredienteGateway IngredienteRepository { get; }
        public IIngredientesLancheGateway IngredientesLancheRepository { get; }
        public ILancheGateway LancheRepository { get; }
        public ISobremesaGateway SobremesaRepository { get; }

    }
}
