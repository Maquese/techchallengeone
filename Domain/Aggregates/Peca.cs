using Domain.Entidades;

namespace Domain.Aggregates;

public class Peca
{
    public int Id { get; private set; }
    public string Nome { get;private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public DateTime DataAtualizacao { get; private set; }
    public bool Ativo { get; private set; }
    public int QuantidadeEmEstoque { get; private set; }
    public ICollection<OrdemServico> OrdemServicos { get; private set; }

    public Peca(int id, string nome, string descricao, decimal valor)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("O nome da peça é obrigatório.");
        if (string.IsNullOrEmpty(descricao))
            throw new ArgumentException("A descrição da peça é obrigatória.");
        if (valor < 0)
            throw new ArgumentException("O valor da peça não pode ser negativo.");

        Id = id;
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        DataCadastro = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;
        QuantidadeEmEstoque = 0;
        Ativo = true;
    }

    public void Atualizar(string nome, string descricao, decimal valor)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("O nome da peça é obrigatório.");
        if (string.IsNullOrEmpty(descricao))
            throw new ArgumentException("A descrição da peça é obrigatória.");
        if (valor < 0)
            throw new ArgumentException("O valor da peça não pode ser negativo.");

        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Inativar()
    {
        Ativo = false;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void AdicionarQuantidadeEstoque(int quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("A quantidade a ser adicionada não pode ser negativa.");

        QuantidadeEmEstoque += quantidade;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void DeduzirQuantidadeEstoque(int quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("A quantidade a ser removida não pode ser negativa.");
        if (quantidade > QuantidadeEmEstoque)
            throw new InvalidOperationException("Não é possível remover mais peças do que o estoque disponível.");

        QuantidadeEmEstoque -= quantidade;
        DataAtualizacao = DateTime.UtcNow;
    }

    
}
