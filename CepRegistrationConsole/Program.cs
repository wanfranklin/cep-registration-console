using CepRegistrationConsole.Models;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

try
{
    var client = new HttpClient();
    var cepsCadastrados = new List<ViaCepModel>();
    bool continuar = true;

    while (continuar)
    {
        Console.Clear();
        Console.WriteLine("=== Sistema de Cadastro de CEPs ===");
        Console.WriteLine("1. Cadastrar CEP");
        Console.WriteLine("2. Consultar CEP");
        Console.WriteLine("3. Remover CEP");
        Console.WriteLine("4. Visualizar CEPs cadastrados");
        Console.WriteLine("5. Exportar dados para Excel");
        Console.WriteLine("6. Exportar dados para CSV");
        Console.WriteLine("7. Exportar dados para JSON");
        Console.WriteLine("8. Remover todos os CEPs");
        Console.WriteLine("9. Sair");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine() ?? string.Empty;

        switch (opcao)
        {
            case "1":
                await CadastrarCep(client, cepsCadastrados);
                break;
            case "2":
                await ConsultarCep(client, cepsCadastrados);
                break;
            case "3":
                RemoverCep(cepsCadastrados);
                break;
            case "4":
                VisualizarCepsCadastrados(cepsCadastrados);
                break;
            case "5":
                ExportarParaExcel(cepsCadastrados);
                break;
            case "6":
                ExportarParaCsv(cepsCadastrados);
                break;
            case "7":
                ExportarParaJson(cepsCadastrados);
                break;
            case "8":
                RemoverTodosCeps(cepsCadastrados);
                break;
            case "9":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida. Pressione Enter para tentar novamente...");
                Console.ReadLine();
                break;
        }
    }

    Console.WriteLine("Saindo...");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}

void RemoverTodosCeps(List<ViaCepModel> cepsCadastrados)
{
    cepsCadastrados.Clear();
    
    Console.WriteLine("Todos os CEPs foram removidos da memória.");

    string filePath = "ceps_cadastrados.json";

    if (File.Exists(filePath))
    {
        File.Delete(filePath);
        Console.WriteLine("Todos os CEPs foram removidos do arquivo JSON.");
    }
    else
    {
        Console.WriteLine("Arquivo JSON não encontrado.");
    }

    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
}

async Task CadastrarCep(HttpClient client, List<ViaCepModel> cepsCadastrados)
{
    string cep = GetCepFromUser();
    
    var viaCepModel = await TryGetCepDataAsync(client, cep);

    if (viaCepModel != null)
    {
        cepsCadastrados.Add(viaCepModel);
    
        Console.WriteLine("CEP cadastrado com sucesso!");

        AdicionarCepAoJson(viaCepModel);
    }
    else
    {
        Console.WriteLine("Erro ao cadastrar o CEP.");
    }

    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
}

void AdicionarCepAoJson(ViaCepModel viaCepModel)
{
    try
    {
        string filePath = "ceps_cadastrados.json";

        List<ViaCepModel> cepsExistentes = [];

        if (File.Exists(filePath))
        {
            string jsonExistente = File.ReadAllText(filePath);
            
            cepsExistentes = JsonConvert.DeserializeObject<List<ViaCepModel>>(jsonExistente) ?? new List<ViaCepModel>();
        }

        cepsExistentes.Add(viaCepModel);

        string json = JsonConvert.SerializeObject(cepsExistentes, Formatting.Indented);
        
        File.WriteAllText(filePath, json);

        Console.WriteLine("CEP adicionado ao arquivo JSON com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao adicionar CEP ao arquivo JSON: {ex.Message}");
    }
}

async Task ConsultarCep(HttpClient client, List<ViaCepModel> cepsCadastrados)
{
    string cep = GetCepFromUser();
    
    var viaCepModel = await TryGetCepDataAsync(client, cep);

    if (viaCepModel != null)
    {
        Console.WriteLine($"CEP: {viaCepModel.Cep}");
        Console.WriteLine($"Logradouro: {viaCepModel.Logradouro}");
        Console.WriteLine($"Complemento: {viaCepModel.Complemento}");
        Console.WriteLine($"Bairro: {viaCepModel.Bairro}");
        Console.WriteLine($"Localidade: {viaCepModel.Localidade} - {viaCepModel.UF}");
    }
    else
    {
        Console.WriteLine("CEP não encontrado.");
    }

    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
}

void RemoverCep(List<ViaCepModel> cepsCadastrados)
{
    Console.WriteLine("Digite o CEP que deseja remover: ");
    
    string cep = Console.ReadLine()?.Trim() ?? string.Empty;

    var cepRemovido = cepsCadastrados.FirstOrDefault(c => c.Cep == cep);

    if (cepRemovido != null)
    {
        cepsCadastrados.Remove(cepRemovido);

        AtualizarPlanilhaExcel(cepsCadastrados);

        Console.WriteLine($"CEP {cep} removido com sucesso!");
    }
    else
    {
        Console.WriteLine("CEP não encontrado.");
    }

    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
}

void AtualizarPlanilhaExcel(List<ViaCepModel> cepsCadastrados)
{
    try
    {
        string filePath = "Planilha_Ceps.xlsx";
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);

        worksheet.RowsUsed().Delete();

        worksheet.Cell(1, 1).Value = "CEP";
        worksheet.Cell(1, 2).Value = "Logradouro";
        worksheet.Cell(1, 3).Value = "Complemento";
        worksheet.Cell(1, 4).Value = "Bairro";
        worksheet.Cell(1, 5).Value = "Localidade";
        worksheet.Cell(1, 6).Value = "UF";
        worksheet.Cell(1, 7).Value = "Data de Cadastro";

        int row = 2;
        foreach (var cep in cepsCadastrados)
        {
            worksheet.Cell(row, 1).Value = cep.Cep;
            worksheet.Cell(row, 2).Value = cep.Logradouro;
            worksheet.Cell(row, 3).Value = cep.Complemento;
            worksheet.Cell(row, 4).Value = cep.Bairro;
            worksheet.Cell(row, 5).Value = cep.Localidade;
            worksheet.Cell(row, 6).Value = cep.UF;
            worksheet.Cell(row, 7).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            row++;
        }

        workbook.SaveAs(filePath);

        Console.WriteLine("Planilha atualizada com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao atualizar a planilha: {ex.Message}");
    }
}

void VisualizarCepsCadastrados(List<ViaCepModel> cepsCadastrados)
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

    Console.WriteLine("Pressione Enter para continuar...");
    Console.ReadLine();
}

void ExportarParaExcel(List<ViaCepModel> cepsCadastrados)
{
    if (cepsCadastrados.Count == 0)
    {
        Console.WriteLine("Nenhum dado para exportar.");
        return;
    }

    using var workbook = new XLWorkbook();

    var worksheet = workbook.Worksheets.Add("CEPs");

    worksheet.Cell(1, 1).Value = "CEP";
    worksheet.Cell(1, 2).Value = "Logradouro";
    worksheet.Cell(1, 3).Value = "Complemento";
    worksheet.Cell(1, 4).Value = "Bairro";
    worksheet.Cell(1, 5).Value = "Localidade";
    worksheet.Cell(1, 6).Value = "UF";
    worksheet.Cell(1, 7).Value = "Data de Cadastro";

    int row = 2;

    foreach (var cep in cepsCadastrados)
    {
        worksheet.Cell(row, 1).Value = cep.Cep;
        worksheet.Cell(row, 2).Value = cep.Logradouro;
        worksheet.Cell(row, 3).Value = cep.Complemento;
        worksheet.Cell(row, 4).Value = cep.Bairro;
        worksheet.Cell(row, 5).Value = cep.Localidade;
        worksheet.Cell(row, 6).Value = cep.UF;
        worksheet.Cell(row, 7).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        row++;
    }

    workbook.SaveAs("Planilha_Ceps.xlsx");

    Console.WriteLine("Dados exportados para Excel com sucesso!");
}

void ExportarParaCsv(List<ViaCepModel> cepsCadastrados)
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

void ExportarParaJson(List<ViaCepModel> cepsCadastrados)
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

string GetCepFromUser()
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

async Task<ViaCepModel?> TryGetCepDataAsync(HttpClient client, string cep)
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