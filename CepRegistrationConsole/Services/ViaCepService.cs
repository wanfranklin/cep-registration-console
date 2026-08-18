using CepRegistrationConsole.Models;
using Newtonsoft.Json;

namespace CepRegistrationConsole.Services;

public class ViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ViaCepModel?> GetCepDataAsync(string cep)
    {
        string viaCepUrl = $"https://viacep.com.br/ws/{cep}/json/";

        HttpResponseMessage response = await _httpClient.GetAsync(viaCepUrl);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro: {response.StatusCode}");
            return null;
        }

        string responseBody = await response.Content.ReadAsStringAsync();

        if (responseBody.Contains("\"erro\": true"))
        {
            Console.WriteLine("CEP não localizado.");
            return null;
        }

        return JsonConvert.DeserializeObject<ViaCepModel>(responseBody);
    }
}
