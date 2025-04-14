using CepRegistrationConsole.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CepRegistrationConsole;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var client = new HttpClient();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CEP");

            int row = 1;
            bool continueAdding = true;

            while (continueAdding)
            {
                string cep = GetCepFromUser();

                var viaCepModel = await TryGetCepDataAsync(client, cep);

                if (viaCepModel != null)
                {
                    PopulateWorksheet(worksheet, row++, viaCepModel);

                    Console.WriteLine("Deseja cadastrar outro CEP? (S/N)");
                    string input = Console.ReadLine().Trim().ToUpper();

                    continueAdding = input == "S";
                }
                else
                {
                    Console.WriteLine("CEP não localizado ou erro na consulta.");
                }
            }

            workbook.SaveAs("Planilha_Ceps.xlsx");
            Console.WriteLine("Planilha salva com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    private static string GetCepFromUser()
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

    private static async Task<ViaCepModel?> TryGetCepDataAsync(HttpClient client, string cep)
    {
        string viaCepUrl = $"https://viacep.com.br/ws/{cep}/json/";

        HttpResponseMessage response = await client.GetAsync(viaCepUrl);

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

    private static void PopulateWorksheet(IXLWorksheet worksheet, int row, ViaCepModel viaCepModel)
    {
        worksheet.Cell(row, 1).Value = viaCepModel.Cep;
        worksheet.Cell(row, 2).Value = viaCepModel.Logradouro;
        worksheet.Cell(row, 3).Value = viaCepModel.Complemento;
        worksheet.Cell(row, 4).Value = viaCepModel.Bairro;
        worksheet.Cell(row, 5).Value = viaCepModel.Localidade;
        worksheet.Cell(row, 6).Value = viaCepModel.UF;
    }
}