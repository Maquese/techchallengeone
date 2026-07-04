using Aplication.Models;
using Domain.Aggregates;
using Domain.Exceptions;
using Aplication.Interfaces;

namespace Aplication.Services;

public class OrcamentoAppServiceImp
{
    private readonly OrcamentoRepository _orcamentoRepository;
    private readonly OrdemServicoRepository _ordemServicoRepository;

    public OrcamentoAppServiceImp(OrcamentoRepository orcamentoRepository, OrdemServicoRepository ordemServicoRepository)
    {
        _orcamentoRepository = orcamentoRepository;
        _ordemServicoRepository = ordemServicoRepository;
    }


}
