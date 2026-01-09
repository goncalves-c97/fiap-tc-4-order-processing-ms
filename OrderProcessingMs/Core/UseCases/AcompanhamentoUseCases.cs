using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Gateways;

namespace Core.UseCases
{
    public static class AcompanhamentoUseCases
    {
        public static Task<IEnumerable<Acompanhamento>> GetAllAcompanhamentos(IAcompanhamentoGateway acompanhamentoGateway)
        {
            if (acompanhamentoGateway == null)
                throw new ArgumentNullException(nameof(acompanhamentoGateway), "Acompanhamento gateway cannot be null.");

            return acompanhamentoGateway.GetAll();
        }

        public static Task<Acompanhamento?> GetAcompanhamentoById(IAcompanhamentoGateway acompanhamentoGateway, int idAcompanhamento)
        {
            if (acompanhamentoGateway == null)
                throw new ArgumentNullException(nameof(acompanhamentoGateway), "Acompanhamento gateway cannot be null.");
            if (idAcompanhamento <= 0)
                throw new ArgumentException("ID do acompanhamento deve ser maior que zero.", nameof(idAcompanhamento));

            return acompanhamentoGateway.GetById(idAcompanhamento);
        }

        public static Task<Acompanhamento> InsertAcompanhamento(IAcompanhamentoGateway acompanhamentoGateway, NomePrecoDto nomePrecoDto)
        {
            if (acompanhamentoGateway == null)
                throw new ArgumentNullException(nameof(acompanhamentoGateway), "Acompanhamento gateway cannot be null.");
            if (nomePrecoDto == null)
                throw new ArgumentNullException(nameof(nomePrecoDto), "NomePrecoDto cannot be null.");

            return acompanhamentoGateway.Insert(nomePrecoDto);
        }

        public static async Task UpdateAcompanhamento(IAcompanhamentoGateway acompanhamentoGateway, int idAcompanhamento, NomePrecoDto nomePrecoDto)
        {
            if (acompanhamentoGateway == null)
                throw new ArgumentNullException(nameof(acompanhamentoGateway), "Acompanhamento gateway cannot be null.");
            if (idAcompanhamento <= 0)
                throw new ArgumentException("ID do acompanhamento deve ser maior que zero.", nameof(idAcompanhamento));

            Acompanhamento? acompanhamento = await acompanhamentoGateway.GetById(idAcompanhamento)
                ?? throw new KeyNotFoundException($"Acompanhamento com ID {idAcompanhamento} não encontrado.");

            acompanhamento.Nome = nomePrecoDto.Nome;
            acompanhamento.Preco = nomePrecoDto.Preco;

            await acompanhamentoGateway.Update(acompanhamento);
        }

        public static async Task DeleteAcompanhamento(IAcompanhamentoGateway acompanhamentoGateway, int idAcompanhamento)
        {
            if (acompanhamentoGateway == null)
                throw new ArgumentNullException(nameof(acompanhamentoGateway), "Acompanhamento gateway cannot be null.");
            if (idAcompanhamento <= 0)
                throw new ArgumentException("ID do acompanhamento deve ser maior que zero.", nameof(idAcompanhamento));

            Acompanhamento? acompanhamento = await acompanhamentoGateway.GetById(idAcompanhamento);

            // Já foi deletado ou não existe
            if (acompanhamento == null)
                return;

            await acompanhamentoGateway.Delete(acompanhamento);
        }
    }
}
