using Domain.Aggregates;
using Domain.Interfaces;

namespace Infra.Repository;

public class OrcamentoRepositoryImp : BaseRepositoryImp<Orcamento>, OrcamentoRepository
{
    public OrcamentoRepositoryImp(EFContext context) : base(context)
    {
    }
}
