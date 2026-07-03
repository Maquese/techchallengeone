using Domain.Entidades;
using Domain.Interfaces;

namespace Infra.Repository;

public class ServicoRepositoryImp : BaseRepositoryImp<Servico>, ServicoRepository
{
    public ServicoRepositoryImp(EFContext context) : base(context)
    {
    }
}
