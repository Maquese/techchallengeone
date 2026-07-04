using Domain.Entidades;
using Aplication.Interfaces;

namespace Infra.Repository;

public class ServicoRepositoryImp : BaseRepositoryImp<Servico>, ServicoRepository
{
    public ServicoRepositoryImp(EFContext context) : base(context)
    {
    }
}
