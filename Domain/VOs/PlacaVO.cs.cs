using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.VOs;

public class PlacaVO
{
    private static readonly HashSet<string> _placasRegistradas = new();

    public string Valor { get; private set; }

    protected PlacaVO() { }

    public PlacaVO(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new DomainException("Placa inválida. Não pode ser vazia.");

        var normalizada = placa.ToUpper().Trim();

        if (!Validar(normalizada))
            throw new DomainException("Placa inválida. Deve estar no padrão brasileiro (AAA-1234 ou ABC1D23).");

        if (!_placasRegistradas.Add(normalizada))
            throw new DomainException("Placa já registrada. Deve ser única.");

        Valor = normalizada;
    }

    private bool Validar(string placa)
    {
        // Padrão antigo: AAA-1234
        var regexAntigo = new Regex(@"^[A-Z]{3}-\d{4}$");

        // Padrão Mercosul: ABC1D23
        var regexNovo = new Regex(@"^[A-Z]{3}\d[A-Z]\d{2}$");

        return regexAntigo.IsMatch(placa) || regexNovo.IsMatch(placa);
    }

    public override bool Equals(object obj)
    {
        if (obj is not PlacaVO other) return false;
        return Valor == other.Valor;
    }

    public override int GetHashCode() => Valor.GetHashCode();
}
