using Domain.Exceptions;

namespace Domain.VOs;

public class CpfVO
{
    public string Numero { get;private set; }

    protected CpfVO() { }
    public CpfVO(string numero)
    {
        if (!Validar(numero))
            throw new DomainException("CPF inválido");
        Numero = numero;
    }

    private bool Validar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return false;

        // Remove caracteres não numéricos
        numero = new string(numero.Where(char.IsDigit).ToArray());

        if (numero.Length != 11)
            return false;

        // Elimina CPFs inválidos conhecidos (todos dígitos iguais)
        if (new string(numero[0], numero.Length) == numero)
            return false;

        // Calcula primeiro dígito
        int[] pesos1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (numero[i] - '0') * pesos1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        // Calcula segundo dígito
        int[] pesos2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (numero[i] - '0') * pesos2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // Confere dígitos
        return numero[9] - '0' == digito1 && numero[10] - '0' == digito2;
    }

    public override string ToString() => Numero;
}