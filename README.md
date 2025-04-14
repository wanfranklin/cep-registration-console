# 📦 Cadastro de CEPs - Aplicação Console

Este projeto é uma aplicação console desenvolvida em **C#** que permite consultar, cadastrar, remover e exportar dados de **endereços a partir de CEPs** utilizando a API pública [ViaCEP](https://viacep.com.br/). Os dados podem ser salvos em **planilha Excel**, **CSV** e **JSON**, com funcionalidades adicionais de gerenciamento de CEPs.

---

## ✨ Funcionalidades

- 🔍 **Consulta de CEP**  
  O usuário insere um CEP e a aplicação valida o formato antes da consulta.

- 🌐 **Consulta via API ViaCEP**  
  Os dados do CEP são obtidos via requisição à API ViaCEP.

- 📝 **Cadastro de múltiplos CEPs**  
  É possível cadastrar quantos CEPs desejar, com persistência dos dados em memória e JSON.

- 🧾 **Exportação dos dados**  
  Os CEPs cadastrados podem ser exportados para:
  - Excel (`.xlsx`)
  - CSV (`.csv`)
  - JSON (`.json`)

- ❌ **Remoção de CEP**  
  Permite remover um CEP específico da memória e atualizar o arquivo Excel.

- 🧹 **Remover todos os CEPs**  
  Limpa a memória e também remove os dados salvos no arquivo JSON.

- 📋 **Visualizar todos os CEPs cadastrados**  
  Exibe no console todos os dados armazenados.

- 💾 **Armazenamento automático em JSON**  
  A cada novo CEP cadastrado, o sistema atualiza automaticamente o arquivo `ceps_cadastrados.json`.

---

## 🛠️ Tecnologias Utilizadas

- **C#**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- **ClosedXML** (Excel `.xlsx`)
- **Newtonsoft.Json** (JSON)
- **API ViaCEP** (https://viacep.com.br/)

---

## ▶️ Como Rodar o Projeto

### Requisitos

- .NET 8.0 ou superior instalado
- Visual Studio, VS Code ou outra IDE compatível
- Internet ativa (para consultas na API ViaCEP)

### Instalar pacotes necessários

Execute os comandos abaixo no terminal da raiz do projeto:

```bash
dotnet add package ClosedXML
dotnet add package Newtonsoft.Json