using Aplication.Models;
using Domain;
using Domain.Aggregates;
using Domain.InfraInterfaces;

namespace Aplication.Services;

public class PecaAppServiceImp
{
    private readonly PecaRepository _pecaRepository;

    public PecaAppServiceImp(PecaRepository pecaRepository)
    {
        _pecaRepository = pecaRepository;
    }

    public void AddPeca(PecaModel peca)
    {
        var novaPeca = new Peca
        (
            0,
            peca.Nome,
            peca.Descricao,
            peca.Valor
        );
        _pecaRepository.Adicionar(novaPeca);
    }
}
    