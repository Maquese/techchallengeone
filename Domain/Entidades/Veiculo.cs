using Domain.Aggregates;

namespace Domain.Entidades;

public class Veiculo
{
    protected Veiculo() { }

    public Veiculo(string placa, string modelo, string marca, int ano, int clienteId)
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
        ClienteId = clienteId;
    }

    public int Id { get; private set; }
    public string Placa { get;private set; }
    public string Modelo { get; private set; }
    public string Marca { get; private set; }
    public int Ano { get; private set; }
    public int ClienteId { get; private set; }
    public Cliente Cliente { get; private set; }
    public ICollection<OrdemServico> OrdemServicos { get; private set; }
}
