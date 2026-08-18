using CepRegistrationConsole.Models;
using ClosedXML.Excel;

namespace CepRegistrationConsole.Export;

public static class ExcelExporter
{
    public static void ExportToExcel(List<ViaCepModel> cepsCadastrados)
    {
        if (cepsCadastrados.Count == 0)
        {
            Console.WriteLine("Nenhum dado para exportar.");
            return;
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("CEPs");

        SetHeaders(worksheet);
        PopulateData(worksheet, cepsCadastrados);

        workbook.SaveAs("Planilha_Ceps.xlsx");
        Console.WriteLine("Dados exportados para Excel com sucesso!");
    }

    public static void UpdateExcel(List<ViaCepModel> cepsCadastrados)
    {
        try
        {
            string filePath = "Planilha_Ceps.xlsx";
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);

            worksheet.RowsUsed().Delete();

            SetHeaders(worksheet);
            PopulateData(worksheet, cepsCadastrados);

            workbook.SaveAs(filePath);
            Console.WriteLine("Planilha atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar a planilha: {ex.Message}");
        }
    }

    private static void SetHeaders(IXLWorksheet worksheet)
    {
        worksheet.Cell(1, 1).Value = "CEP";
        worksheet.Cell(1, 2).Value = "Logradouro";
        worksheet.Cell(1, 3).Value = "Complemento";
        worksheet.Cell(1, 4).Value = "Bairro";
        worksheet.Cell(1, 5).Value = "Localidade";
        worksheet.Cell(1, 6).Value = "UF";
        worksheet.Cell(1, 7).Value = "Data de Cadastro";
    }

    private static void PopulateData(IXLWorksheet worksheet, List<ViaCepModel> ceps)
    {
        int row = 2;
        foreach (var cep in ceps)
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
    }
}
