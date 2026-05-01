using System.Security.Cryptography.X509Certificates;
using Aplication.Models;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;
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



    
    public async Task<ServicoModel> AdicionarServico(ServicoModel servico)
    {
        var servicoEntity = new Servico
        (
            descricao: servico.Descricao,
            valor: servico.Valor,
            tempoEstimado: servico.TempoEstimado
        );
        
        await _servicoRepository.Adicionar(servicoEntity);
        return servico;
    }
    public async Task<ServicoModel> BuscarServico(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            return null;
        }

        return new ServicoModel
        {
            Id = servico.Id,
            Descricao = servico.Descricao,
            Valor = servico.Valor,
            TempoEstimado = servico.TempoEstimado
        };
    }

    public async Task AtualizarServico(UpdateServicoModel servicoModel)
    {
        var servico = await _servicoRepository.ObterPorId(servicoModel.Id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        servico.Atualizar(servicoModel.Descricao, servicoModel.Valor, servicoModel.TempoEstimado);

        await _servicoRepository.Atualizar(servico);
    }

    public async Task InativarServico(int id)
    {
        var servico = await _servicoRepository.ObterPorId(id);
        if (servico == null)
        {
            throw new DomainException("Serviço não encontrado");
        }

        await _servicoRepository.Inativar(servico);
    }

    public async Task<List<ServicoModel>> ListarServicosAtivos()
    {
        var servicos = await _servicoRepository.ListarAtivos();
        return servicos.Select(s => new ServicoModel
        {
            Id = s.Id,
            Descricao = s.Descricao,
            Valor = s.Valor,
            TempoEstimado = s.TempoEstimado
        }).ToList();
    }

}
