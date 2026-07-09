# Diretrizes para Resolução: Ampliação da Cobertura de Testes Unitários

Este arquivo contém orientações para que o assistente de IA resolva a issue de ampliação de testes unitários.

## Escopo do Problema
O único teste existente cobre o hashing de senhas. Métodos de parser de string de conexão e regras de cálculo de pontos do bolão não estão cobertos por testes unitários, criando o risco de regressões futuras.

## Plano de Ação para o Assistente de IA
1. **Refatoração Inicial**:
   - Localize o método `CalculatePoints` em [Admin.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/Admin.razor#L438-L464).
   - Extraia essa lógica para uma classe estática dedicada em helpers (ex: `src/Bolao/Helpers/ScoreCalculator.cs`), para permitir que a classe de teste a chame de forma limpa.
2. **Criação de Testes do ScoreCalculator**:
   - Crie a classe `ScoreCalculatorTests.cs` no projeto de testes [Bolao.Tests](file:///c:/dev/Bolao/tests/Bolao.Tests/Bolao.Tests.csproj).
   - Escreva fatos/teorias (`[Fact]` e `[Theory]`) com os cenários:
     - Placar real `2x1` e palpite `2x1` deve dar 5 pontos.
     - Placar real `3x0` e palpite `1x0` (acertou vencedor, errou placar) deve dar 3 pontos.
     - Placar real `1x1` e palpite `0x0` (acertou empate, errou placar) deve dar 3 pontos.
     - Placar real `0x2` e palpite `2x1` (errou vencedor e placar) deve dar 0 pontos.
3. **Criação de Testes do ConnectionStringParser**:
   - Crie a classe `ConnectionStringParserTests.cs`.
   - Adicione testes verificando o comportamento para strings válidas no formato `postgres://user:pass@host:port/db`, fallback de strings comuns do SQL Server ou SQLite, e entradas nulas ou vazias.
4. **Execução**:
   - Rode `dotnet test` e certifique-se de que todos os novos testes passem.
