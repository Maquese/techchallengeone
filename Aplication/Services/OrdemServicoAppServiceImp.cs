using System.Security.Cryptography.X509Certificates;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Aplication.Services;

public class OrdemServicoAppServiceImp
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ServicoRepository _servicoRepository;

    public OrdemServicoAppServiceImp(OrdemServicoRepository ordemServicoRepository, ServicoRepository servicoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _servicoRepository = servicoRepository;
    }

    public async Task<OrdemServico> AdicionarOrdemServico(OrdemServicoModel ordemServico)
    {
        var ordemServicoEntity = new OrdemServico(
            dataAbertura: DateTime.Now,
            dataFechamento: null,
            veiculoId: ordemServico.VeiculoId
        );

        await _ordemServicoRepository.Adicionar(ordemServicoEntity);
        return ordemServicoEntity;
    }

    public async Task<ServicoModel> AdicionarServico(ServicoModel servico)
    {
        var servicoEntity = new Servico
        (
            id: servico.Id ?? 0,
            descricao: servico.Descricao,
            valor: servico.Valor,
            tempoEstimado: servico.TempoEstimado
        );
        
        await _servicoRepository.Adicionar(servicoEntity);
        return servico;
    }

    public async Task<string> AtribuirMecanico(AtribuiMecanicoModel atribuiEmDiagnostico)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(atribuiEmDiagnostico.OrdemServicoId);
        if (ordemServico == null)
        {
            return $"Ordem de serviço com ID {atribuiEmDiagnostico.OrdemServicoId} não encontrada.";
        }

        ordemServico.OSEmDiagnostico(atribuiEmDiagnostico.MecanicoAtribuido);
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Mecânico '{atribuiEmDiagnostico.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmDiagnostico.OrdemServicoId}.";
    }

    public async Task<string> AtribuirMecanicoExecucao(AtribuiMecanicoModel atribuiEmReparo)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(atribuiEmReparo.OrdemServicoId);
        if (ordemServico == null)
        {
            return $"Ordem de serviço com ID {atribuiEmReparo.OrdemServicoId} não encontrada.";
        }

        ordemServico.EmExecucao(atribuiEmReparo.MecanicoAtribuido);
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Mecânico '{atribuiEmReparo.MecanicoAtribuido}' atribuído à ordem de serviço ID {atribuiEmReparo.OrdemServicoId}.";
    }

    public async Task<string> FinalizarOrdemServico(int ordemServicoId)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(ordemServicoId);
        if (ordemServico == null)
        {
            return $"Ordem de serviço com ID {ordemServicoId} não encontrada.";
        }
        ordemServico.FinalizarOrdemServico();
        await _ordemServicoRepository.Atualizar(ordemServico);

        return $"Ordem de serviço ID {ordemServicoId} finalizada com sucesso.";
    }
}
