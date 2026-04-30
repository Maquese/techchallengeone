using Domain.Entidades;
using Domain.VOs;

namespace Domain.Aggregates;

public class Cliente : IEntity
{
    protected Cliente() { }
    public Cliente(CpfVO cpf, string nome, EmailVO email, CelularVO celular)
    {
        this.Cpf = cpf;
        Nome = nome;
        Email = email;
        Celular = celular;
    }
    public int Id { get; private set; }
    public CpfVO Cpf { get; private set; }
    public string Nome { get; private set; }
    public EmailVO Email { get; private set; }
    public CelularVO Celular { get; private set; }
    public IList<Veiculo> Veiculos { get; private set; }
}
