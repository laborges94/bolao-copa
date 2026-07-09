# Diretrizes para Resolução: Automação de Git Hooks locais com Husky.Net

Este arquivo contém orientações para que o assistente de IA resolva a issue de git hooks locais com Husky.Net.

## Escopo do Problema
Permitir que desenvolvedores enviem código formatado incorretamente ou que quebre o build atrasa o pipeline de integração. A automação local impede a criação de commits ou pushes de códigos defeituosos.

## Plano de Ação para o Assistente de IA
1. Instale a ferramenta dotnet `Husky.Net` globalmente ou localmente na solução seguindo a documentação oficial.
2. Adicione as dependências de tarefas no arquivo `package.json` (se houver node) ou execute diretamente os comandos dotnet do Husky para criar o diretório `.husky/` na raiz do repositório.
3. Configure os hooks:
   - **pre-commit**: Crie uma tarefa que rode `dotnet format --verify-no-changes`.
   - **pre-push**: Crie uma tarefa que execute `dotnet build` e `dotnet test`.
4. Garanta que o comando de inicialização `dotnet husky install` seja executado de forma automática no build/restore do projeto C# (configurando um target no `Bolao.csproj` ou adicionando um passo no fluxo de onboarding).
5. Faça um commit teste local simulando um erro para certificar-se de que o Git bloqueia a ação corretamente.
