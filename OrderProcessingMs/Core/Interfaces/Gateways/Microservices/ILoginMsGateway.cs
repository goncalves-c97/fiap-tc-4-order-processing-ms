using Core.Entities;

namespace Core.Interfaces.Gateways.Microservices
{
    public interface ILoginMsGateway
    {
        Task<Cliente> GetClienteById(int idCliente, string token);
    }
}
