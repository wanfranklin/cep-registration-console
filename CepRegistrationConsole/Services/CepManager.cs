using CepRegistrationConsole.Models;
using Newtonsoft.Json;

namespace CepRegistrationConsole.Services;

public class CepManager
{
    private readonly string _jsonFilePath = "ceps_cadastrados.json";

    public List<ViaCepModel> LoadFromJson()
    {
        if (!File.Exists(_jsonFilePath))
            return new List<ViaCepModel>();

        string json = File.ReadAllText(_jsonFilePath);
        return JsonConvert.DeserializeObject<List<ViaCepModel>>(json) ?? new List<ViaCepModel>();
    }

    public void SaveToJson(List<ViaCepModel> ceps)
    {
        string json = JsonConvert.SerializeObject(ceps, Formatting.Indented);
        File.WriteAllText(_jsonFilePath, json);
    }

    public void AddCep(ViaCepModel cep, List<ViaCepModel> cepsCadastrados)
    {
        cepsCadastrados.Add(cep);
        SaveToJson(cepsCadastrados);
        Console.WriteLine("CEP cadastrado com sucesso!");
    }

    public bool RemoveCep(string cep, List<ViaCepModel> cepsCadastrados)
    {
        var cepRemovido = cepsCadastrados.FirstOrDefault(c => c.Cep == cep);

        if (cepRemovido != null)
        {
            cepsCadastrados.Remove(cepRemovido);
            SaveToJson(cepsCadastrados);
            return true;
        }

        return false;
    }

    public void RemoveAll(List<ViaCepModel> cepsCadastrados)
    {
        cepsCadastrados.Clear();

        if (File.Exists(_jsonFilePath))
            File.Delete(_jsonFilePath);
    }

    public void DisplayCeps(List<ViaCepModel> cepsCadastrados)
    {
        Console.WriteLine("\n=== CEPs Cadastrados ===");

        if (cepsCadastrados.Count == 0)
        {
            Console.WriteLine("Nenhum CEP cadastrado.");
            return;
        }

        foreach (var cep in cepsCadastrados)
        {
            Console.WriteLine($"CEP: {cep.Cep}");
            Console.WriteLine($"Logradouro: {cep.Logradouro}");
            Console.WriteLine($"Complemento: {cep.Complemento}");
            Console.WriteLine($"Bairro: {cep.Bairro}");
            Console.WriteLine($"Localidade: {cep.Localidade} - {cep.UF}");
            Console.WriteLine($"Data de Cadastro: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n");
        }
    }
}
