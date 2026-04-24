using Domain.InfraInterfaces;
using Domain.Entidades;

namespace Infra;

public class VeiculoRepositoryImp : BaseRepositoryImp<Veiculo>, VeiculoRepository
{
    public VeiculoRepositoryImp(EFContext context) : base(context)
    {
    }
}

