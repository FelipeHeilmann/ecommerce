using Domain.Customers.Error;
using System.Text.RegularExpressions;

namespace Domain.Customers.VO;

public record CPF
{
    public string Value { get; init; }

    public CPF(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) throw new InvalidCPF();

        var cleanedCPF = Regex.Replace(cpf, @"\D", "");

        if (cleanedCPF.Length != 11) throw new InvalidCPF();

        if (!ValidateDigit(cleanedCPF)) throw new InvalidCPF();

        Value = cleanedCPF;
    }
    private bool ValidateDigit(string cpf)
    {
        int[] numbers = new int[11];
        for (int i = 0; i < 11; i++)
        {
            numbers[i] = int.Parse(cpf[i].ToString());
        }

        int sum1 = 0;
        for (int i = 0; i < 9; i++)
        {
            sum1 += numbers[i] * (10 - i);
        }

        int remainder1 = sum1 % 11;
        int digit1 = remainder1 < 2 ? 0 : 11 - remainder1;

        if (numbers[9] != digit1)
            return false;

        int sum2 = 0;
        for (int i = 0; i < 10; i++)
        {
            sum2 += numbers[i] * (11 - i);
        }

        int remainder2 = sum2 % 11;
        int digit2 = remainder2 < 2 ? 0 : 11 - remainder2;

        if (numbers[10] != digit2)
            return false;

        return true;
    }
}
