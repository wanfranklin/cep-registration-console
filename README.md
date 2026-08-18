# Cadastro de CEPs - Aplicação Console

Este projeto é uma aplicação console desenvolvida em **C#** que permite consultar, cadastrar, remover e exportar dados de **endereços a partir de CEPs** utilizando a API pública [ViaCEP](https://viacep.com.br/). Os dados podem ser salvos em **planilha Excel**, **CSV** e **JSON**, com funcionalidades adicionais de gerenciamento de CEPs.

---

## Funcionalidades

- **Consulta de CEP** — O usuário insere um CEP e a aplicação valida o formato antes da consulta.
- **Consulta via API ViaCEP** — Os dados do CEP são obtidos via requisição à API ViaCEP.
- **Cadastro de múltiplos CEPs** — É possível cadastrar quantos CEPs desejar, com persistência dos dados em memória e JSON.
- **Exportação dos dados** — Os CEPs cadastrados podem ser exportados para Excel (`.xlsx`), CSV (`.csv`) e JSON (`.json`).
- **Remoção de CEP** — Permite remover um CEP específico da memória e atualizar o arquivo Excel.
- **Remover todos os CEPs** — Limpa a memória e também remove os dados salvos no arquivo JSON.
- **Visualizar todos os CEPs cadastrados** — Exibe no console todos os dados armazenados.
- **Armazenamento automático em JSON** — A cada novo CEP cadastrado, o sistema atualiza automaticamente o arquivo `ceps_cadastrados.json`.

---

## Tecnologias Utilizadas

- **C#**
- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download)
- **ClosedXML** (geração de planilhas Excel `.xlsx`)
- **Newtonsoft.Json** (serialização/deserialização JSON)
- **API ViaCEP** (https://viacep.com.br/)

---

## Estrutura do Projeto

```
cep-registration-console/
├── CepRegistrationConsole.sln
├── README.md
├── LICENSE.txt
├── .gitignore
└── CepRegistrationConsole/
    ├── CepRegistrationConsole.csproj
    ├── Program.cs
    ├── Menu.cs
    ├── Models/
    │   └── ViaCepModel.cs
    ├── Services/
    │   ├── ViaCepService.cs
    │   └── CepManager.cs
    ├── Export/
    │   ├── ExcelExporter.cs
    │   ├── CsvExporter.cs
    │   └── JsonExporter.cs
    └── Utils/
        └── InputValidator.cs
```

---

## Como Rodar o Projeto

### Requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download) ou superior instalado
- Visual Studio, VS Code ou outra IDE compatível
- Internet ativa (para consultas na API ViaCEP)

### Instalar pacotes necessários

Os pacotes já estão referenciados no arquivo `.csproj`. Caso precise reinstalar:

```bash
dotnet restore
```

Ou instale manualmente:

```bash
dotnet add package ClosedXML
dotnet add package Newtonsoft.Json
```

### Executar o projeto

Na raiz do projeto (onde está o `.sln`):

```bash
dotnet run --project CepRegistrationConsole
```

Ou dentro da pasta do projeto:

```bash
cd CepRegistrationConsole
dotnet run
```

---

## Uso

Ao executar o sistema, um menu será apresentado no console:

```
=== Sistema de Cadastro de CEPs ===
1. Cadastrar CEP
2. Consultar CEP
3. Remover CEP
4. Visualizar CEPs cadastrados
5. Exportar dados para Excel
6. Exportar dados para CSV
7. Exportar dados para JSON
8. Remover todos os CEPs
9. Sair
```

### Exemplo de uso

1. Escolha a opção `1` para cadastrar um CEP
2. Digite o CEP no formato `XXXXXXXX` (ex: `01001000`)
3. O sistema consulta a ViaCEP e armazena os dados automaticamente
4. Os dados são salvos automaticamente em `ceps_cadastrados.json`

---

## Arquivos Gerados

| Arquivo                     | Descrição                           |
| --------------------------- | ----------------------------------- |
| `ceps_cadastrados.json`     | Dados cadastrados em formato JSON   |
| `ceps_cadastrados.csv`      | Dados exportados em formato CSV     |
| `Planilha_Ceps.xlsx`        | Dados exportados em planilha Excel  |

---

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE.txt](LICENSE.txt) para mais detalhes.
