🚀 Billing System API
=====================

Uma API REST robusta desenvolvida em **.NET 10** para gestão de faturamento, clientes e produtos, com integração de dados externos. Este projeto foi desenvolvido como resolução de desafio técnico para demonstrar boas práticas de arquitetura, testes e engenharia de software.

🛠️ Tecnologias Utilizadas
--------------------------

*   **Framework:** .NET 10.0 (LTS)
    
*   **Banco de Dados:** PostgreSQL
    
*   **ORM:** Entity Framework Core 10
    
*   **Documentação:** Swagger (OpenAPI) configurado com as melhores práticas
    
*   **Padrões Arquiteturais:** Clean Architecture, CQRS, Repository Pattern, Unit of Work
    
*   **Bibliotecas Principais:**
    *   `MediatR` (Mensageria e CQRS)
        
    *   `FluentValidation` (Validações fluentes e Pipeline Behaviors)
        
    *   `Bogus` (Geração de massa de dados falsos)
        
    *   `Moq` (Mocking de interfaces para testes)
        
    *   `xUnit` (Testes automatizados)
        

* * *

🏗️ Arquitetura e Tomada de Decisões
-------------------------------------------------------

O projeto foi desenhado focando na manutenibilidade, testabilidade e escalabilidade. Abaixo estão os principais pilares técnicos aplicados:

### 1. Clean Architecture & CQRS

O código está dividido em camadas estritas (`Api`, `Application`, `Domain`, `Infrastructure`). A camada de Domínio é isolada de tecnologias externas. O padrão **CQRS** (via MediatR) foi utilizado para separar completamente as operações de leitura (Queries) das operações de escrita (Commands), garantindo que as regras de negócio de mutação de estado não poluam as extrações de dados.

### 2. Soft Delete e Integridade de Dados Financeiros

Em sistemas de gestão e faturamento, o histórico é sagrado e a integridade referencial não pode ser quebrada. Ao "deletar" um Cliente ou Produto, a API não executa um `DELETE` físico no banco de dados, mas sim um **Soft Delete** (atualizando uma coluna `DeletedAt`). Isso garante que as notas fiscais antigas continuem intactas e auditáveis. Para que isso não polua o código, foi configurado um **Global Query Filter no Entity Framework Core** que intercepta todas as consultas automaticamente e ignora registros apagados de forma totalmente transparente para o desenvolvedor.

### 3. Internacionalização (i18n) e Mensageria Centralizada

A API foi desenhada para ser global. Foi implementado um `CultureMiddleware` customizado que intercepta o cabeçalho `Accept-Language` da requisição HTTP e adapta o idioma do sistema em tempo real. Além disso, nenhuma mensagem de erro ou validação contém _hardcode_ (texto chumbado no código); todas as mensagens estão unificadas em arquivos de recursos (`.resx`). Isso garante extrema facilidade de manutenção e já provê suporte nativo a múltiplos idiomas (atualmente fluente em Inglês e Português `pt-BR`).

### 4. Tratamento Global de Exceções & Validações Interceptadas

Os _Controllers_ foram mantidos extremamente limpos (Thin Controllers). O `FluentValidation` valida as requisições antes mesmo de chegarem aos _Handlers_ utilizando um _Pipeline Behavior_ do MediatR. Caso haja erro, exceções customizadas (`ErrorOnValidationException`, `ErrorOnSyncBillingException`) são lançadas e interceptadas por um **Exception Filter** global, padronizando as respostas de erro (HTTP 400, 404, 207, 500) no formato JSON, sem sujar o fluxo principal da aplicação.

### 5. Convenções REST e Model Binding (Snake_case)

Para aderir aos padrões mais modernos de APIs públicas e Front-ends, foi implementada uma convenção na API que transforma e trafega todo o payload JSON em `snake_case`, incluindo o _Model Binding_ nativo do ASP.NET para _Query Strings_, mantendo o código C# intacto no padrão nativo `PascalCase` e `camelCase`.

### 6. Resiliência de Sincronização (Multi-Status 207)

O requisito exigia validar a existência de clientes e produtos ao sincronizar notas. Na funcionalidade de _Sync_, em vez de falhar toda a carga caso uma nota externa apresente dados ausentes, foi implementado um mecanismo resiliente: a API processa e salva as notas válidas e retorna o código HTTP **207 (Multi-Status)** para informar o sucesso parcial, relatando de forma granular nas exceções quais notas falharam por ausência de registros (Cliente ou Produto).

### 7. Conversão Automática de Datas (UTC no EF Core)

O provedor do PostgreSQL é estrito quanto a Timezones. Para resolver isso globalmente, foi injetado um `ValueConverter` direto na sobrescrita do Entity Framework Core (`ConfigureConventions`). Isso blinda a aplicação contra falhas de fuso horário, garantindo que todas as datas que transitam entre a aplicação e a base de dados sejam tratadas e salvas perfeitamente em `UTC`.

### 8. Desenvolvimento Guiado a Testes (TDD / Integração)

O desenvolvimento seguiu rigorosas práticas de testes. Além de testes de Casos de Uso e Validadores, o grande diferencial está nos **Testes de Integração com WebApplicationFactory**. Um banco de dados `InMemory` gerido dinamicamente garante a verificação fim-a-fio das rotas HTTP de forma isolada, com os serviços externos de faturamento totalmente _Mockados_. Isso impede a concorrência de banco de dados e valida com precisão as respostas, os cabeçalhos de paginação e o _status code_ exato de cada operação.

* * *

⚙️ Configuração e Execução (Setup)
----------------------------------

### Pré-requisitos

*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado.
    
*   [Docker](https://www.docker.com/) e Docker Compose instalados (opcional, para rodar o PostgreSQL de forma automatizada).
    
*   IDE de sua preferência (Visual Studio 2022, VS Code, Rider).
    

### Passo a Passo

1.  **Clone o repositório:**

``` bash
git clone https://github.com/matheusfsc28/ca-backend-test.git 
cd BillingSystem
```

2.  **Suba o banco de dados via Docker:** O projeto conta com um arquivo `docker-compose.yml` pré-configurado na raiz.

``` bash
docker-compose up -d
```

Nota: O PostgreSQL ficará acessível na porta `5433` com usuário, senha e banco definidos no compose.

3.  **String de Conexão:** Verifique o arquivo `src/BillingSystem.Api/appsettings.Development.json`. A _Connection String_ já aponta para o banco de dados local levantado pelo Docker.

4.  Execução da API:

``` bash
cd src/BillingSystem.Api
dotnet run
```

5.  Acesso ao Swagger:
Abra o navegador no endereço exibido no terminal (ex: `https://localhost:7035/swagger/index.html`). A documentação interativa conta com exibição de tempo de latência, metadados ricos e sumários de todas as rotas (via XML Comments).

📡 Endpoints Disponíveis
------------------------

A API disponibiliza documentação rica via Swagger, mas os principais fluxos são:
**Clientes (Customers)**
*   `POST /api/v1/customer` - Cadastra um novo cliente (Obrigatório Id, Name, Email, Address).
    
*   `GET /api/v1/customer/{id}` - Busca cliente por ID.
    
*   `GET /api/v1/customer` - Lista clientes de forma paginada.
    
*   `PUT /api/v1/customer/{id}` - Atualiza um cliente existente.
    
*   `DELETE /api/v1/customer/{id}` - Remove (Soft Delete) um cliente da base.
    
**Produtos (Products)**
*   `POST /api/v1/product` - Cadastra um produto (Obrigatório Id, Nome).
    
*   `GET /api/v1/product/{id}` - Busca produto por ID.
    
*   `GET /api/v1/product` - Lista produtos de forma paginada.
    
*   `PUT /api/v1/product/{id}` - Atualiza um produto existente.
    
*   `DELETE /api/v1/product/{id}` - Remove (Soft Delete) um produto.
    
**Faturamento (Billings)**
*   `POST /api/v1/billing/sync` - Processo de integração. Vai até a _MockAPI_ externa, confere se os IDs de Clientes e Produtos vindos dela já existem na nossa base de dados local, e sincroniza as Notas (_Billings_) e seus Itens (_Billing Lines_). Retorna alertas detalhados em caso de falhas nas regras de negócio.
    
*   `GET /api/v1/billing` - Extrai os dados do faturamento importado de forma paginada e permite a aplicação de filtros dinâmicos via URL.
    
*   `GET /api/v1/billing/{id}` - Busca um faturamento detalhado pelo seu ID interno.