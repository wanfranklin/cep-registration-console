using CepRegistrationConsole.Export;
using CepRegistrationConsole.Services;
using CepRegistrationConsole.Utils;

namespace CepRegistrationConsole;

public class Menu
{
    private readonly HttpClient _httpClient;
    private readonly ViaCepService _viaCepService;
    private readonly CepManager _cepManager;
    private readonly List<Models.ViaCepModel> _cepsCadastrados;

    public Menu(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _viaCepService = new ViaCepService(httpClient);
        _cepManager = new CepManager();
        _cepsCadastrados = _cepManager.LoadFromJson();
    }

    public void Run()
    {
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
                    CadastrarCep().Wait();
                    break;
                case "2":
                    ConsultarCep().Wait();
                    break;
                case "3":
                    RemoverCep();
                    break;
                case "4":
                    VisualizarCepsCadastrados();
                    break;
                case "5":
                    ExportarParaExcel();
                    break;
                case "6":
                    ExportarParaCsv();
                    break;
                case "7":
                    ExportarParaJson();
                    break;
                case "8":
                    RemoverTodosCeps();
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

    private async Task CadastrarCep()
    {
        string cep = InputValidator.GetValidCepFromUser();
        var viaCepModel = await _viaCepService.GetCepDataAsync(cep);

        if (viaCepModel != null)
        {
            _cepManager.AddCep(viaCepModel, _cepsCadastrados);
        }
        else
        {
            Console.WriteLine("Erro ao cadastrar o CEP.");
        }

        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }

    private async Task ConsultarCep()
    {
        string cep = InputValidator.GetValidCepFromUser();
        var viaCepModel = await _viaCepService.GetCepDataAsync(cep);

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

    private void RemoverCep()
    {
        string cep = InputValidator.GetCepForRemoval();

        if (_cepManager.RemoveCep(cep, _cepsCadastrados))
        {
            ExcelExporter.UpdateExcel(_cepsCadastrados);
            Console.WriteLine($"CEP {cep} removido com sucesso!");
        }
        else
        {
            Console.WriteLine("CEP não encontrado.");
        }

        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }

    private void VisualizarCepsCadastrados()
    {
        _cepManager.DisplayCeps(_cepsCadastrados);
        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }

    private void ExportarParaExcel()
    {
        ExcelExporter.ExportToExcel(_cepsCadastrados);
    }

    private void ExportarParaCsv()
    {
        CsvExporter.ExportToCsv(_cepsCadastrados);
    }

    private void ExportarParaJson()
    {
        JsonExporter.ExportToJson(_cepsCadastrados);
    }

    private void RemoverTodosCeps()
    {
        _cepManager.RemoveAll(_cepsCadastrados);
        Console.WriteLine("Todos os CEPs foram removidos.");

        if (File.Exists("Planilha_Ceps.xlsx"))
            File.Delete("Planilha_Ceps.xlsx");

        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }
}
