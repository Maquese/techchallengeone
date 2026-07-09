using Aplication.Interfaces;
using Application.Models.Requests;
using Domain.Aggregates;
using Domain.Entidades;
using Domain.Exceptions;

namespace Application.UseCases.OrdensServico;

public class FinalizarDiagnosticoOSHandler
{
    private readonly OrdemServicoRepository _ordemServicoRepository;
    private readonly ServicoRepository _servicoRepository;
    private readonly ItemEstoqueRepository _itensEstoqueRepository;
    private readonly OrcamentoRepository _orcamentoRepository;

    public FinalizarDiagnosticoOSHandler(OrdemServicoRepository ordemServicoRepository, ServicoRepository servicoRepository, ItemEstoqueRepository itensEstoqueRepository, OrcamentoRepository orcamentoRepository)
    {
        _ordemServicoRepository = ordemServicoRepository;
        _servicoRepository = servicoRepository;
        _itensEstoqueRepository = itensEstoqueRepository;
        _orcamentoRepository = orcamentoRepository; 
    }

      public async Task Handle(DiagnosticoFinalizadoRequest diagnosticoFinalizadoModel)
    {
        var ordemServico = await _ordemServicoRepository.ObterPorId(diagnosticoFinalizadoModel.Id);
        if (ordemServico == null)
        {
            throw new DomainException($"Ordem de serviço para o veículo ID {diagnosticoFinalizadoModel.Id} não encontrada.");
        }

        if(ordemServico.Status != "Em diagnóstico")
        {
            throw new DomainException($"Ordem de serviço com ID {ordemServico.Id} não está no status 'Em diagnóstico' para finalização do diagnóstico.");
        }

        var itensEstoque = diagnosticoFinalizadoModel.ItensEstoque.Select(item => new OrdemServicoItemEstoque
        (diagnosticoFinalizadoModel.Id, item.id, item.quantidade)).ToList();
       
        ordemServico.OSDiagnosticada(itensEstoque);
        var orcamento = new Orcamento(
            diagnosticoFinalizadoModel.Id, 
            await CalcularValorTotalOrcamento(diagnosticoFinalizadoModel.ItensEstoque, ordemServico.Servicos?.Select(x => x.Id).ToList()),
            "obs");
        await _ordemServicoRepository.Atualizar(ordemServico);
        await _orcamentoRepository.Adicionar(orcamento);
    }

    private async Task<decimal> CalcularValorTotalOrcamento(List<AddItensOrdemServicoRequest> itensEstoque, List<int> servicosIds)
    {
        var servicos = await _servicoRepository.ListarPorIds(servicosIds);

        var itens = await _itensEstoqueRepository.ListarPorIds(itensEstoque.Select(i => i.id).ToList());

        return servicos.Sum(s => s.Valor) + itens.Sum(i => i.Valor * itensEstoque.First(e => e.id == i.Id).quantidade);
    }
}
