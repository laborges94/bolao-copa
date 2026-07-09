# Diretrizes para Resolução: Otimização das Migrações e Inicialização do Banco em Produção

Este arquivo contém orientações para que o assistente de IA resolva a issue de otimização de migrações.

## Escopo do Problema
O [Program.cs](file:///c:/dev/Bolao/src/Bolao/Program.cs#L55-L59) executa migrations e seeding a cada inicialização da aplicação web. Em ambientes Cloud (como Render, Heroku) ou Docker escaláveis horizontalmente com múltiplas instâncias concorrentes, isso gera travamentos graves de concorrência de escrita no banco de dados.

## Plano de Ação para o Assistente de IA
1. Crie uma lógica no [Program.cs](file:///c:/dev/Bolao/src/Bolao/Program.cs) para ler argumentos da linha de comando ou variáveis de ambiente.
   Exemplo:
   ```csharp
   bool runMigrationsOnly = args.Contains("--migrate") || 
                            Environment.GetEnvironmentVariable("RUN_MIGRATIONS") == "true";
   ```
2. Se a flag estiver presente no startup:
   - Execute o escopo de migrações e seeding.
   - Encerre o processo imediatamente com código de sucesso (`Environment.Exit(0)`), impedindo que o servidor web suba e comece a ouvir requisições HTTP.
3. Se a flag NÃO estiver ativa:
   - Apenas monte a aplicação normalmente, ignorando a etapa de migrations e inicialização.
4. Ajuste os scripts de deploy/Dockerfile:
   - Configure a etapa de pré-deploy (ou um script script `entrypoint.sh` no Docker) para rodar `dotnet Bolao.dll --migrate` antes de de fato iniciar o contêiner principal da aplicação (`dotnet Bolao.dll`).
