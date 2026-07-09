# Diretrizes para Resolução: Validação de Formatação de Código (CSharpier/dotnet-format) no CI

Este arquivo contém orientações para que o assistente de IA resolva a issue de validação de formatação de código no CI.

## Escopo do Problema
O repositório não valida a formatação de arquivos C# no servidor de Integração Contínua (CI). Códigos com padrões de recuo, espaçamento e chaves diferentes podem ser mergeados.

## Plano de Ação para o Assistente de IA
1. Decida qual ferramenta utilizar:
   - **dotnet-format**: Nativo do SDK do .NET, fácil de rodar diretamente.
   - **CSharpier**: Formatador de código C# opinativo semelhante ao Prettier. Necessita ser instalado como ferramenta dotnet local ou referenciado por pacote NuGet.
2. Adicione um arquivo `.editorconfig` na raiz da solução caso decida por `dotnet-format`, definindo as regras de estilo de código (ex: tamanho de tabs, chaves em novas linhas, etc.).
3. Edite o arquivo do GitHub Actions [.NET CI](file:///c:/dev/Bolao/.github/workflows/ci.yml) para adicionar a validação após a etapa de restauração de dependências.
   Exemplo usando dotnet-format:
   ```yaml
   - name: Verificar formatação do código
     run: dotnet format --verify-no-changes --no-restore
   ```
4. Se o validador falhar devido a arquivos malformatados no repositório, rode `dotnet format` localmente para corrigir tudo, commite os arquivos formatados e valide se a action do CI passa.
