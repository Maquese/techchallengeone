using Domain.Aggregates;
using Domain.Entidades;
using Domain.InfraInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class OrdemServicoRepositoryImp : BaseRepositoryImp<OrdemServico>, OrdemServicoRepository
{
    public OrdemServicoRepositoryImp(EFContext context) : base(context)
    {
        
    }

    public override async Task<OrdemServico> ObterPorId(int id)
    {
        return await _context.Set<OrdemServico>()
            .Include(os => os.Servicos)
            .Include(os => os.OrdemServicoItensEstoque)
                .ThenInclude(osi => osi.ItemEstoque)
            .Include(os => os.Orcamentos)
            .FirstOrDefaultAsync(os => os.Id == id);
    }

}
