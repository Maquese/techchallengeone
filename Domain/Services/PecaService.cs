using Domain.Aggregates;
using Domain.InfraInterfaces;

namespace Domain;

public class PecaService
{
    private readonly PecaRepository _repository;

    public PecaService(PecaRepository pecaRepository)
    {
        _repository = pecaRepository;
    }

    public void AddPeca(Peca peca)
    {
        _repository.Adicionar(peca);
    }
}
