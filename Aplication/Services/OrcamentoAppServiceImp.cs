using Aplication.Models;
using Domain.Aggregates;
using Domain.InfraInterfaces;

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

    public async Task<int> AddOrcamento(AddOrcamentoModel model)
    {
        var ordemServico = _ordemServicoRepository.ObterPorId(model.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new Exception($"Ordem de serviço com ID {model.OrdemServicoId} não encontrada.");
        }
        var orcamento = new Orcamento
        (model.OrdemServicoId, 100,"obs");
        await _orcamentoRepository.Adicionar(orcamento);
        return orcamento.Id;
    }

    public async Task AprovarOrcamento(int orcamentoId)
    {
        var orcamento = await _orcamentoRepository.ObterPorId(orcamentoId);
        if (orcamento == null)
        {
            throw new Exception($"Orçamento com ID {orcamentoId} não encontrado.");
        }

        var ordemServico = await _ordemServicoRepository.ObterPorId(orcamento.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new Exception($"Ordem de serviço com ID {orcamento.OrdemServicoId} não encontrada.");
        }

        orcamento.AprovarOrcamento();
        ordemServico.OSAprovada();
        await _orcamentoRepository.Atualizar(orcamento);
    }

    public async Task PagarOrcamento(int orcamentoId)
    {
        var orcamento = await _orcamentoRepository.ObterPorId(orcamentoId);
        if (orcamento == null)
        {
            throw new Exception($"Orçamento com ID {orcamentoId} não encontrado.");
        }

        var ordemServico = await _ordemServicoRepository.ObterPorId(orcamento.OrdemServicoId);
        if (ordemServico == null)
        {
            throw new Exception($"Ordem de serviço com ID {orcamento.OrdemServicoId} não encontrada.");
        }

        orcamento.MarcarOrcamentoPago();
        ordemServico.OrdemServicoEntregue();
        await _orcamentoRepository.Atualizar(orcamento);
    }

}
