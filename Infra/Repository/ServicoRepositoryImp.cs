using Domain.Entidades;
using Domain.InfraInterfaces;

namespace Infra.Repository;

public class ServicoRepositoryImp : BaseRepositoryImp<Servico>, ServicoRepository
{
    public ServicoRepositoryImp(EFContext context) : base(context)
    {
    }
}
