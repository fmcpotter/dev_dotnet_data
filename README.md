# dev_dotnet_data — Protótipo API de Vendas (123Vendas)

Este repositório contém um protótipo de API REST para o domínio de Vendas da empresa 123Vendas, implementado em .NET (C#) com enfoque em DDD, camadas separadas e boas práticas (SOLID, Clean Code, DRY).

Principais características:
- CRUD completo para Vendas (Compra)
- Registro de itens, quantidades, valores unitários, descontos, valor total por item e total da venda
- Informações desnormalizadas para referência a entidades externas (Cliente, Produto, Filial) — External Identities
- Estado Cancelado / Não Cancelado em venda e itens
- Emissão de eventos internos (log de eventos para CompraCriada, CompraAlterada, CompraCancelada, ItemCancelado)
- Estrutura em camadas: Api, Domain, Data
- Logging: Serilog
- Testes unitários: xUnit, FluentAssertions, Bogus, NSubstitute
- Banco de dados em memória para o protótipo (EF Core InMemory) — fácil de trocar para SQL Server/Postgres
- Recomenda-se uso de Git Flow e Commits semânticos no histórico de commits

Como rodar localmente
1. Requisitos
   - .NET 7 SDK (ou .NET 8 se preferir, ajustar TargetFramework)
   - Git
   - (Opcional) Docker, se quiser rodar contêiner externo

2. Clonar
   git clone https://github.com/fmcpotter/dev_dotnet_data.git
   cd dev_dotnet_data

3. Compilar e executar API
   - Via CLI:
     dotnet restore
     dotnet build
     dotnet run --project src/Api/Api.csproj

   A API por padrão roda em http://localhost:5000 e https://localhost:5001 (Kestrel). Swagger está habilitado em ambiente de desenvolvimento: /swagger

4. Testes
   dotnet test

Documentação da API (endpoints principais)
- GET /api/sales — listar vendas (filtros simples)
- GET /api/sales/{id} — obter venda por id
- POST /api/sales — criar venda
- PUT /api/sales/{id} — atualizar venda
- DELETE /api/sales/{id} — cancelar venda (marca como cancelada)
- POST /api/sales/{id}/items/{itemId}/cancel — cancelar item

Modelos principais (resumo)
- Sale
  - Id (GUID)
  - SaleNumber (string)
  - Date (DateTime)
  - ClientExternalId (string)
  - ClientDescription (string) // desnormalização
  - BranchExternalId (string)
  - BranchDescription (string)
  - TotalAmount (decimal)
  - IsCancelled (bool)
  - Items: collection de SaleItem

- SaleItem
  - Id (GUID)
  - ProductExternalId (string)
  - ProductDescription (string)
  - Quantity (decimal)
  - UnitPrice (decimal)
  - Discount (decimal)
  - Total (decimal)
  - IsCancelled (bool)

Eventos (implementados via logging)
- CompraCriada
- CompraAlterada
- CompraCancelada
- ItemCancelado

Observações
- Projeto organizado para fácil extensão: trocar persistência, adicionar mensageria real, autenticação, etc.
- Testes de integração com Testcontainers podem ser adicionados posteriormente (desejável).

Estrutura de pastas (resumida)
- src/Api : projeto Web API
- src/Domain : entidades, serviços de domínio, eventos, interfaces
- src/Data : implementação EF Core, repositórios
- tests/UnitTests : testes unitários
- README.md, .gitignore

Licença
- Código entregue como exemplo de avaliação.