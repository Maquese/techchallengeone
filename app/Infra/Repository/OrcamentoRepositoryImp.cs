using Domain.Aggregates;
using Aplication.Interfaces;

namespace Infra.Repository;

public class OrcamentoRepositoryImp : BaseRepositoryImp<Orcamento>, OrcamentoRepository
{
    public OrcamentoRepositoryImp(EFContext context) : base(context)
    {
    }
}
