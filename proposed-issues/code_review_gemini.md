# Diretrizes para Resolução: Pull Request Code Review Automatizado com Gemini API

Este arquivo contém orientações para que o assistente de IA resolva a issue de Code Review automatizado usando Gemini.

## Escopo do Problema
Melhorar a qualidade de código em tempo de desenvolvimento. Um bot rodando no CI pode analisar o diff de um PR e comentar possíveis melhorias diretamente no GitHub antes da revisão humana.

## Plano de Ação para o Assistente de IA
1. Crie uma GitHub Action em `.github/workflows/ai-code-review.yml` disparada em eventos de `pull_request`.
2. Configure a action para obter a API Key do Gemini via segredos do repositório (`${{ secrets.GEMINI_API_KEY }}`).
3. Escreva um script em Python ou Node.js que:
   - Obtenha o diff do Pull Request utilizando a API REST do GitHub.
   - Faça uma chamada à API do Gemini utilizando o SDK oficial (recomenda-se modelo `gemini-2.5-flash` devido ao custo e velocidade).
   - O prompt deve instruir o modelo a se comportar como um Tech Lead em .NET e Blazor, focando em segurança, boas práticas e performance, sem ser pedante em formatações que o linter já resolve.
   - Processe a resposta e faça chamadas de comentário na API do GitHub usando o `GITHUB_TOKEN` para publicar o resultado no PR.
4. Teste a Action simulando um Pull Request com um trecho de código propositalmente ineficiente.
