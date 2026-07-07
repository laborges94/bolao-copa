# Plano de Implementação: Publicação do Bolão no Render com PostgreSQL

Este plano descreve as etapas necessárias para configurar a aplicação Blazor (ASP.NET Core / .NET 10) para rodar em produção no Render usando **PostgreSQL** (com o serviço Render Postgres), enquanto mantém a facilidade do desenvolvimento local utilizando **SQLite**.

---

## Estratégia de Banco de Dados

- **Desenvolvimento Local**: Continuará utilizando **SQLite** (`bolao.db`) para facilidade de desenvolvimento e sem necessidade de configurar um banco local.
- **Produção (Render)**: Utilizará **PostgreSQL** por meio do serviço nativo Render Postgres. A aplicação detectará a presença da variável de ambiente `DATABASE_URL` (injetada pelo Render) e usará o provedor PostgreSQL automaticamente.
- **Inicialização do Banco**:
  - Para SQLite local: Continuará rodando `context.Database.Migrate()`.
  - Para PostgreSQL em produção: Utilizará `context.Database.EnsureCreated()` para inicializar a estrutura de tabelas automaticamente, evitando incompatibilidade de tipos de dados das migrations geradas originalmente para SQLite.

---

## Proposed Changes

### 1. Dependências do Projeto

#### [MODIFY] [Bolao.csproj](file:///c:/dev/Bolao/src/Bolao/Bolao.csproj)
Adicionar o pacote `Npgsql.EntityFrameworkCore.PostgreSQL` compatível com o EF Core do projeto.

---

### 2. Utilitários e Helpers

#### [NEW] [ConnectionStringParser.cs](file:///c:/dev/Bolao/src/Bolao/Helpers/ConnectionStringParser.cs)
Criar um utilitário para converter a URI do banco de dados fornecida pelo Render (no formato `postgres://...` ou `postgresql://...`) em uma string de conexão compatível com o Npgsql do EF Core.

---

### 3. Ajuste de Configuração da Aplicação

#### [MODIFY] [Program.cs](file:///c:/dev/Bolao/src/Bolao/Program.cs)
Alterar a inicialização do `DbContext` para:
1. Buscar por uma string de conexão PostgreSQL na variável de ambiente `DATABASE_URL` ou na seção `ConnectionStrings:DefaultConnection`.
2. Se configurada, fazer o parse da URI e usar `.UseNpgsql(...)`.
3. Caso contrário, usar `.UseSqlite(...)` para o banco local.

#### [MODIFY] [SeedData.cs](file:///c:/dev/Bolao/src/Bolao/Data/SeedData.cs)
Alterar a inicialização para evitar que as migrations do SQLite rodem no PostgreSQL:
- Se o provedor for SQLite, executa `context.Database.Migrate()`.
- Se o provedor for PostgreSQL, executa `context.Database.EnsureCreated()`.

---

### 4. Configuração Docker

#### [NEW] [Dockerfile](file:///c:/dev/Bolao/Dockerfile)
Criar um Dockerfile multi-stage otimizado para compilar e executar o projeto no .NET 10.

#### [NEW] [.dockerignore](file:///c:/dev/Bolao/.dockerignore)
Criar um `.dockerignore` para evitar enviar pastas de compilação locais (`bin/`, `obj/`) e arquivos locais de banco (`*.db`) para a imagem Docker.

---

## Verification Plan

### Testes de Compilação
- Compilar o projeto localmente para garantir que o pacote Npgsql e a classe ConnectionStringParser não geram erros de build:
  ```powershell
  dotnet build src/Bolao/Bolao.csproj
  ```

### Verificação de Inicialização Local
- Executar o projeto localmente em modo SQLite para verificar se as migrações continuam rodando sem problemas:
  ```powershell
  dotnet run --project src/Bolao/Bolao.csproj
  ```

---

## Passo a Passo Completo para Publicação no Render

Após a aprovação deste plano e a execução das alterações no código, você precisará seguir estes passos no painel do Render para publicar a aplicação:

### Passo 1: Criar o Banco de Dados PostgreSQL no Render
1. Acesse o painel do [Render](https://dashboard.render.com).
2. Clique em **New +** e selecione **PostgreSQL**.
3. Preencha as informações:
   - **Name**: `bolao-db`
   - **Database**: `bolao`
   - **User**: (deixe gerar automaticamente ou digite um de sua preferência)
   - **Region**: Selecione a mesma região onde hospedará a aplicação (ex: `Oregon (US West)` ou `Frankfurt (EU Central)`).
4. Escolha o plano **Free** e clique em **Create Database**.
5. Salve a **Internal Database URL** (que será usada para conectar o app ao banco internamente na rede do Render).

### Passo 2: Criar o Web Service da Aplicação no Render
1. No painel do Render, clique em **New +** e selecione **Web Service**.
2. Escolha **Build and deploy from a Git repository**.
3. Selecione o repositório do seu Bolão (conecte sua conta do GitHub se ainda não o fez).
4. Preencha os campos de configuração:
   - **Name**: `bolao-app`
   - **Region**: A mesma escolhida para o PostgreSQL.
   - **Branch**: `main` (ou a branch onde as alterações foram aplicadas).
   - **Runtime**: **Docker**.
5. Selecione o plano **Free**.

### Passo 3: Configurar as Variáveis de Ambiente no Render
1. No formulário de criação do Web Service (ou na aba **Env Groups / Environment** após criar), clique em **Add Environment Variable**.
2. Adicione as seguintes variáveis:
   - **Key**: `DATABASE_URL` | **Value**: (Cole a *Internal Database URL* do banco de dados criado no Passo 1).
   - **Key**: `ASPNETCORE_ENVIRONMENT` | **Value**: `Production`
3. Clique em **Create Web Service** (ou Save se estiver alterando depois).

O Render iniciará o build da imagem Docker da sua aplicação e fará o deploy automático. O banco será populado na primeira execução pelo `SeedData` usando as credenciais do Postgres.
