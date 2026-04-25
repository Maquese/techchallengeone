using Domain.Aggregates;
using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Infra.Repository;

public class OrdemServicoRepositoryImp : BaseRepositoryImp<OrdemServico>, OrdemServicoRepository
{
    public OrdemServicoRepositoryImp(EFContext context) : base(context)
    {
        
    }

}
