# Diretrizes para Resolução: Mitigação de Vulnerabilidade no Pacote SQLitePCLRaw

Este arquivo contém orientações para que o assistente de IA resolva a issue de vulnerabilidade no SQLitePCLRaw.

## Escopo do Problema
O pipeline do NuGet identifica a vulnerabilidade alta [GHSA-2m69-gcr7-jv3q](https://github.com/advisories/GHSA-2m69-gcr7-jv3q) associada ao pacote `SQLitePCLRaw.lib.e_sqlite3` versão `2.1.11`. O projeto usa o SQLite para ambiente de desenvolvimento local.

## Plano de Ação para o Assistente de IA
1. Inspecione [Bolao.csproj](file:///c:/dev/Bolao/src/Bolao/Bolao.csproj) e verifique a dependência `Microsoft.EntityFrameworkCore.Sqlite`.
2. Verifique se existe alguma versão mais estável e recente disponível que atualize o subpacote `SQLitePCLRaw` para uma versão não vulnerável (>= 2.1.12 ou compatível).
3. Atualize a referência no arquivo `.csproj`. Se o pacote do EF Core SQLite não resolver implicitamente a dependência, adicione a referência direta ao pacote `SQLitePCLRaw.bundle_e_sqlite3` ou correspondente com a versão corrigida:
   ```xml
   <PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.12" />
   ```
4. Rode `dotnet restore` e em seguida `dotnet list package --vulnerable` para validar que a vulnerabilidade desapareceu.
5. Compile e execute os testes automatizados com `dotnet test` para validar o comportamento da aplicação.
