# Diretrizes para Resolução: Adição de Templates de Pull Request e Issues no GitHub

Este arquivo contém orientações para que o assistente de IA resolva a issue de templates de GitHub.

## Escopo do Problema
O repositório não possui padronização para abertura de Pull Requests e relato de erros (Issues). É necessário criar templates que guiem o usuário ao fornecer informações.

## Plano de Ação para o Assistente de IA
1. Crie a pasta `.github/ISSUE_TEMPLATE/` na raiz do repositório.
2. Adicione os seguintes arquivos markdown nessa pasta:
   - `bug_report.md`: template com seções de comportamento esperado, comportamento atual, passos para reproduzir e capturas de tela.
   - `feature_request.md`: template com descrição do problema, solução proposta e alternativas consideradas.
3. Adicione na pasta `.github/` o arquivo:
   - `pull_request_template.md`: contendo checklist de qualidade, breve resumo das alterações, issues vinculadas e tipo de alteração (feat, fix, docs, etc.).

## Referências Úteis
* Documentação oficial do GitHub sobre [Issue Templates](https://docs.github.com/en/communities/using-templates-to-encourage-useful-issues-and-pull-requests/configuring-issue-templates-for-your-repository).
* Documentação oficial sobre [PR Templates](https://docs.github.com/en/communities/using-templates-to-encourage-useful-issues-and-pull-requests/creating-a-pull-request-template-for-your-repository).
