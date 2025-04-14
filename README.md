# Cadastro de CEPs - Aplicação Console

Este projeto é uma aplicação console em C# que consulta dados de um CEP fornecido pelo usuário através da API ViaCEP, e salva os dados de endereço em uma planilha Excel. O usuário pode cadastrar múltiplos CEPs e a planilha gerada será salva com todas as informações cadastradas.

## Funcionalidades

- **Consulta de CEP**: O programa solicita ao usuário que insira um CEP e valida o formato.
- **Consulta via API**: A aplicação utiliza a API pública ViaCEP para obter informações sobre o CEP (logradouro, bairro, cidade, UF, etc).
- **Cadastro de múltiplos CEPs**: Após o cadastro de um CEP, o programa pergunta ao usuário se ele deseja cadastrar outro. O processo pode ser repetido várias vezes.
- **Geração de planilha Excel**: Os dados de cada CEP consultado são salvos em uma planilha Excel (`Planilha_Ceps.xlsx`), com as seguintes informações:
  - CEP
  - Logradouro
  - Complemento
  - Bairro
  - Localidade (Cidade)
  - UF (Estado)

## Tecnologias Utilizadas

- **C#**: Linguagem de programação utilizada para desenvolver a aplicação.
- **ClosedXML**: Biblioteca para trabalhar com arquivos Excel (.xlsx).
- **Newtonsoft.Json**: Biblioteca para manipulação de JSON.
- **API ViaCEP**: API pública para consultar dados de endereço a partir do CEP.

## Como Rodar o Projeto

### Requisitos

- .NET 8.0 ou superior instalado em sua máquina.
- IDE de sua preferência (Visual Studio, Visual Studio Code, etc).
- Pacotes NuGet:
  - `ClosedXML` (para manipulação de arquivos Excel)
  - `Newtonsoft.Json` (para deserialização de JSON)

### Passos para Executar

1. Clone o repositório para sua máquina local:

   ```bash
   git clone https://github.com/seu-usuario/cep-registration-console.git
   cd cep-registration-console

