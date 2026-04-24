using Domain.InfraInterfaces;
using Domain.Aggregates;
namespace Infra.Repository;

public class ClienteRepositoryImp : BaseRepositoryImp<Cliente>, ClienteRepository
{
    public ClienteRepositoryImp(EFContext context) : base(context)
    {
    }

    public Cliente ObterPorCpf(string cpf)
    {
        return _context.Clientes.FirstOrDefault(c => c.Cpf.Numero == cpf);
    }
}
