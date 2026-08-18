namespace CepRegistrationConsole.Models;

public class ViaCepModel
{
    public required string Cep { get; set; }
    public required string Logradouro { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Localidade { get; set; }
    public required string UF { get; set; }
}
