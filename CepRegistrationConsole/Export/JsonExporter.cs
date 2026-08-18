using CepRegistrationConsole.Models;
using Newtonsoft.Json;

namespace CepRegistrationConsole.Export;

public static class JsonExporter
{
    public static void ExportToJson(List<ViaCepModel> cepsCadastrados)
    {
        if (cepsCadastrados.Count == 0)
        {
            Console.WriteLine("Nenhum dado para exportar.");
            return;
        }

        string json = JsonConvert.SerializeObject(cepsCadastrados, Formatting.Indented);
        File.WriteAllText("ceps_cadastrados.json", json);
        Console.WriteLine("Dados exportados para JSON com sucesso!");
    }
}
