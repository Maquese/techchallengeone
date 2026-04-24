using Domain.Entidades;
using Domain.VOs;

namespace Domain.Aggregates;

public class Cliente
{
    protected Cliente() { }
    public Cliente(int id, CpfVO cpf, string nome, string email, string celular)
    {
        this.Id = id;
        this.Cpf = cpf;
        Nome = nome;
        Email = email;
        Celular = celular;
    }
    public int Id { get; private set; }
    public CpfVO Cpf { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string  Celular { get; private set; }
    public IList<Veiculo> Veiculos { get; private set; }
}
