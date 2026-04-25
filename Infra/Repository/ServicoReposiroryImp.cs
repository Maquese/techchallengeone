using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Infra.Repository;

public class ServicoReposiroryImp : BaseRepositoryImp<Servico>, ServicoRepository
{
    public ServicoReposiroryImp(EFContext context) : base(context)
    {
    }
}
