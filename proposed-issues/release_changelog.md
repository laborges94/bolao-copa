# Diretrizes para Resolução: Release Automática e Geração de Changelog Semântico

Este arquivo contém orientações para que o assistente de IA resolva a issue de release e changelogs automáticos.

## Escopo do Problema
O controle de versão e geração de tags do projeto é feito de forma manual. Automatizar o processo baseado nos commits diminui o trabalho manual e melhora o rastreamento do projeto.

## Plano de Ação para o Assistente de IA
1. Crie um workflow do GitHub Actions em `.github/workflows/release.yml`.
2. Configure o gatilho para rodar apenas em pushes para a branch principal (`main` ou `master`).
3. Adote o uso da biblioteca `semantic-release` (via action de mercado do GitHub) ou configure `GitVersion` no runner do GitHub.
4. O workflow deve:
   - Fazer checkout do código.
   - Analisar o histórico de commits utilizando o formato Conventional Commits (`feat:`, `fix:`, `perf:`).
   - Determinar a nova versão semântica (Major, Minor ou Patch).
   - Gerar um arquivo `CHANGELOG.md` e commitar opcionalmente no repositório.
   - Criar uma Release tag no GitHub e publicar as notas de versão contendo o resumo dos commits agrupados.
5. Adicione instruções de documentação no README.md explicando o padrão de commit que os desenvolvedores precisam seguir a partir desse momento para que a automação funcione.
