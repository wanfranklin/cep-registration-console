using System.Text.RegularExpressions;

namespace CepRegistrationConsole.Utils;

public static class InputValidator
{
    public static string GetValidCepFromUser()
    {
        string cep;
        while (true)
        {
            Console.WriteLine("Digite o CEP (formato XXXXXXXX): ");
            cep = Console.ReadLine()?.Replace(".", "").Replace("-", "") ?? string.Empty;

            if (Regex.IsMatch(cep, @"^\d{8}$"))
            {
                break;
            }
            else
            {
                Console.WriteLine("Formato do CEP inválido.");
            }
        }

        return cep;
    }

    public static string GetCepForRemoval()
    {
        Console.WriteLine("Digite o CEP que deseja remover: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
