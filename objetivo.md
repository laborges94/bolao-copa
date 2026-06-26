# Objetivo

Crie uma aplicação completa em **ASP.NET Core 10 Blazor Server** utilizando **MudBlazor**, partindo de um projeto Blazor Server vazio.

O objetivo é criar um sistema simples de bolão da Copa do Mundo, focado exclusivamente na fase de **16 avos de final** (32 seleções, 16 partidas).

O projeto deve priorizar simplicidade. Não utilize DDD, CQRS, MediatR, AutoMapper, repositórios, services desnecessários, interfaces ou qualquer boilerplate excessivo.

Toda a aplicação deverá existir em **um único projeto**.

---

# Tecnologias

Utilize:

* .NET 10
* Blazor Server
* MudBlazor
* Entity Framework Core
* SQLite
* EF Core Migrations
* Autenticação simples utilizando sessão (cookie) e Entity Framework Core

---

# Estrutura desejada

Organize apenas em pastas simples.

```
Data
Models
Pages
Components
wwwroot
```

Crie o DbContext dentro da pasta Data.

Não criar camadas Application, Domain, Infrastructure etc.

---

# Banco de dados

Utilize SQLite.

Configure migrations.

O banco deve ser criado automaticamente caso não exista.

---

# Models

Crie apenas as seguintes entidades.

## Usuario

Campos:

* Id
* Nome
* Email
* Senha

Relacionamentos:

* Participantes

Utilizar SHA256 para armazenar o hash da senha.

---

## Bolao

Campos:

* Id
* Nome
* CodigoConvite
* CriadoEm
* CriadoPorId
* Encerrado

Relacionamentos:

* Participantes

---

## Participante

Campos:

* Id
* BolaoId
* UsuarioId
* NomeExibicao
* EntrouEm

Relacionamentos:

* Bolao
* Usuario

---

## Selecao

Campos:

* Id
* Nome
* Sigla
* Bandeira

A tabela será preenchida inicialmente com as 32 seleções classificadas.

---

## Fase

Campos:

* Id
* Nome
* Ordem

Relacionamentos:

* Partidas

Nesta primeira versão será cadastrada apenas uma fase:

* 16 avos de final

A entidade deve existir desde o início para facilitar a expansão futura da aplicação para oitavas, quartas, semifinal e final.

---

## Partida

Campos:

* Id
* Numero
* DataHora
* TimeCasaId
* TimeVisitanteId
* GolsCasa
* GolsVisitante
* Finalizada
* FaseId

Relacionamentos:

* Fase
* TimeCasa
* TimeVisitante

Nesta primeira versão existirão apenas as 16 partidas dos 16 avos.

Não é necessário implementar chaveamento para oitavas, quartas etc.

---

## Palpite

Campos:

* Id
* PartidaId
* ParticipanteId
* GolsCasa
* GolsVisitante
* Pontos

Relacionamentos:

* Partida
* Participante

---

# Regras

Implementar apenas estas regras.

## Palpites

O participante pode editar seu palpite até o horário do jogo.

Após o início da partida:

* impedir edição.

---

## Resultado

Quando o administrador informar o resultado oficial:

* marcar partida como Finalizada
* calcular automaticamente todos os pontos dos palpites daquela partida

---

## Pontuação

Resultado exato

Exemplo:

Resultado:

2 x 1

Palpite:

2 x 1

Pontuação:

5 pontos

---

Acertou apenas vencedor

Resultado:

3 x 1

Palpite:

1 x 0

Pontuação:

3 pontos

---

Acertou empate sem acertar placar

Resultado:

2 x 2

Palpite:

1 x 1

Pontuação:

3 pontos

---

Qualquer outro caso:

0 pontos

---

# Ranking

O ranking deve ser calculado somando os pontos dos palpites.

Ordenação:

1. Maior pontuação
2. Nome do participante

Não é necessário armazenar pontuação total.

---

# Seed inicial

Criar um SeedData contendo:

* A fase "16 avos de final";
* As 32 seleções da Copa;
* As 16 partidas vinculadas à fase cadastrada.

Não é necessário consumir API externa.

---

# Autenticação

Implementar um sistema de autenticação simples, sem utilizar ASP.NET Core Identity.

Criar a entidade Usuario e permitir:

* Registro
* Login
* Logout

O login deve validar e-mail e senha cadastrados e manter o usuário autenticado utilizando sessão/cookies do ASP.NET Core.

Não é necessário implementar recuperação de senha, confirmação de e-mail, perfis ou permissões complexas.

Após o login o usuário poderá:

* visualizar seus bolões;
* entrar em um bolão utilizando um código de convite.

---

# Telas

Criar utilizando MudBlazor.

## Home

Página inicial.

---

## Login

---

## Registro

---

## Meus Bolões

Lista dos bolões do usuário.

Botão:

Entrar por código.

---

## Bolão

Exibir:

* nome
* ranking
* participantes

---

## Jogos

Lista de partidas.

Cada card deve exibir:

* bandeiras
* nome das seleções
* data
* horário

Caso o usuário ainda não tenha palpitado:

mostrar campos numéricos para gols.

Caso já tenha:

mostrar valores preenchidos.

Caso o jogo tenha iniciado:

campos desabilitados.

Botão Salvar.

---

## Ranking

Tabela contendo:

Posição

Participante

Pontos

Quantidade de placares exatos

Quantidade de vencedores acertados

---

## Administração

Página protegida.

Permitir:

* cadastrar bolão
* cadastrar partidas
* editar resultado das partidas

Ao salvar um resultado:

recalcular automaticamente todos os palpites daquela partida.

---

# Interface

Utilizar MudBlazor.

Aplicação responsiva.

Tema claro.

Utilizar:

* MudTable
* MudCard
* MudGrid
* MudDialog quando necessário
* Snackbar para mensagens

---

# Código

Priorizar simplicidade.

Evitar abstrações desnecessárias.

Utilizar async/await.

Utilizar LINQ quando fizer sentido.

Criar comentários apenas onde realmente agregarem valor.

Escrever código limpo e organizado.

Sempre que possível utilizar os recursos nativos do Blazor Server e do Entity Framework Core.

Ao finalizar, entregar uma aplicação funcional executando com:

* banco SQLite
* migrations configuradas
* seed funcionando
* login e cadastro de usuários funcionando
* CRUD básico
* ranking funcionando
* cálculo de pontos funcionando
* interface navegável.
