# Bolão da Copa

Este é um sistema simples de bolão para a Copa do Mundo, focado na fase de **16 avos de final** (32 seleções, 16 partidas).

A aplicação foi desenvolvida em **ASP.NET Core 10 Blazor Server** utilizando a biblioteca de componentes **MudBlazor** e banco de dados local **SQLite** com **Entity Framework Core**.

## Tecnologias Utilizadas

* **.NET 10** (Blazor Server)
* **MudBlazor** (Interface de Usuário)
* **Entity Framework Core** (Mapeamento Objeto-Relacional)
* **SQLite** (Banco de dados relacional leve)
* **Autenticação Simples** baseada em cookies/sessão

## Estrutura do Projeto

O projeto segue uma arquitetura simples e direta, mantendo todo o código em um único projeto conforme as especificações:
* `Data/`: Contexto de banco de dados (`BolaoDbContext`), sementes de dados (`SeedData`) e migrações.
* `Models/`: Entidades de domínio (`Usuario`, `Bolao`, `Participante`, `Selecao`, `Fase`, `Partida`, `Palpite`).
* `Components/`: Páginas e componentes Blazor da aplicação (Home, Login, Registro, Meus Bolões, Jogos, Ranking, Administração).
* `wwwroot/`: Arquivos estáticos (imagens, CSS, JS).

## Pré-requisitos

* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado em sua máquina.

## Como Executar

1. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```

2. Aplique as migrações para criar o banco de dados SQLite (`bolao.db`):
   ```bash
   dotnet ef database update
   ```
   *(Nota: O banco de dados também será criado automaticamente na primeira execução caso não exista).*

3. Execute a aplicação:
   ```bash
   dotnet run
   ```

4. Acesse a aplicação no navegador através do endereço exibido no terminal (geralmente `http://localhost:5000` ou `https://localhost:5001`).

## Licença

Este projeto está licenciado sob a licença MIT. Consulte o arquivo `LICENSE` para obter mais detalhes.
