# Diretrizes para Resolução: Assistente de Palpites ("Oráculo da Copa") integrado com Gemini

Este arquivo contém orientações para que o assistente de IA resolva a issue do Assistente de Palpites usando Gemini.

## Escopo do Problema
Trazer inovação e entretenimento à aplicação integrando inteligência artificial. Os usuários poderão pedir análises inteligentes e palpites divertidos de placares para os confrontos cadastrados.

## Plano de Ação para o Assistente de IA
1. Crie uma conta no Google AI Studio para obter a API Key do Gemini.
2. Adicione a chave no `appsettings.json` local e no ambiente de produção do Render.
3. Instale o pacote oficial da biblioteca cliente da API do Gemini (ou use chamadas HTTP diretas via `HttpClient` estruturado) para integração com a API.
4. Crie um serviço `GeminiService.cs` contendo um método `GetMatchPredictionAsync(string timeCasa, string timeVisitante, string fase)` que monta o prompt enviando os dados do jogo atual e pede:
   - Um placar recomendado.
   - Uma breve análise divertida do confronto.
   - Retorno estruturado (JSON ou delimitado) para que a interface consiga ler e processar os dados.
5. No front-end do [BolaoDetail.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/BolaoDetail.razor), adicione o botão "Perguntar ao Oráculo" para cada card de partida que esteja ativo para palpitar.
6. Ao clicar, abra um Modal contendo um carregamento e exiba o texto de simulação da IA. Disponibilize um botão "Usar Placar do Oráculo" que atualiza os inputs da tela com o palpite da IA.
7. Implemente um rate limiting ou trava simples por sessão/usuário para controlar custos com a API.
