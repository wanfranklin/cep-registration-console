using CepRegistrationConsole.Models;
using System.Text;

namespace CepRegistrationConsole.Export;

public static class CsvExporter
{
    public static void ExportToCsv(List<ViaCepModel> cepsCadastrados)
    {
        if (cepsCadastrados.Count == 0)
        {
            Console.WriteLine("Nenhum dado para exportar.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("CEP,Logradouro,Complemento,Bairro,Localidade,UF,Data de Cadastro");

        foreach (var cep in cepsCadastrados)
        {
            sb.AppendLine($"{cep.Cep},{cep.Logradouro},{cep.Complemento},{cep.Bairro},{cep.Localidade},{cep.UF},{DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        }

        File.WriteAllText("ceps_cadastrados.csv", sb.ToString());
        Console.WriteLine("Dados exportados para CSV com sucesso!");
    }
}
