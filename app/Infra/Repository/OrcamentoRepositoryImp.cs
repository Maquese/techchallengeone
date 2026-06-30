using Domain.Aggregates;
using Domain.InfraInterfaces;

namespace Infra.Repository;

public class OrcamentoRepositoryImp : BaseRepositoryImp<Orcamento>, OrcamentoRepository
{
    public OrcamentoRepositoryImp(EFContext context) : base(context)
    {
    }
}
