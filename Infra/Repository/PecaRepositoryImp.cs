using Domain.InfraInterfaces;
using Domain.Aggregates;
namespace Infra.Repository;


public class PecaRepositoryImp : BaseRepositoryImp<Peca>, PecaRepository
{
    public PecaRepositoryImp(EFContext context) : base(context)
    {
    }
}