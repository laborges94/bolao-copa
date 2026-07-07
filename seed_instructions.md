# Instruções de Seed da Copa - Quartas de Final

Este arquivo foi criado para gerenciar o seed do banco de dados para as Quartas de Final da Copa de 2026. Hoje à noite, quando a última seleção das quartas for definida, siga as instruções abaixo para atualizar este arquivo. Em seguida, eu farei as alterações no código da aplicação.

---

## 1. Seleção Faltante (ALTERAR AQUI)

Substitua o valor de `SELECAO_FALTANTE` abaixo com o nome da seleção classificada, sua sigla de 3 letras e seu emoji de bandeira.

```json
{
  "SELECAO_FALTANTE": "SUBSTITUA_AQUI",
  "SIGLA_FALTANTE": "SUB",
  "BANDEIRA_FALTANTE": "🏳️"
}
```

*Exemplo de preenchimento (caso o Brasil se classifique):*
```json
{
  "SELECAO_FALTANTE": "Brasil",
  "SIGLA_FALTANTE": "BRA",
  "BANDEIRA_FALTANTE": "🇧🇷"
}
```

---

## 2. Ações que Eu (IA) Executarei Hoje à Noite

Assim que você preencher o campo acima e me disser "prossiga", eu irei:

1. Ler os dados que você preencheu.
2. Atualizar o arquivo [SeedData.cs](file:///c:/dev/Bolao/src/Bolao/Data/SeedData.cs) para:
   - Limpar qualquer dado de testes anterior no banco de dados (Partidas, Seleções, Fases, Palpites, etc.) para garantir um ambiente limpo.
   - Criar a fase **"Quartas de final"**.
   - Criar as 8 seleções participantes das quartas de final.
   - Criar os 4 confrontos com as datas e horários exatos:
     - **França x Marrocos** (09/07/2026 17:00)
     - **Espanha x Bélgica** (10/07/2026 16:00)
     - **Noruega x Inglaterra** (11/07/2026 18:00)
     - **Argentina x [Sua Seleção]** (11/07/2026 22:00)
   - Manter o usuário administrador padrão (`admin@bolao.com` / `admin123`).
3. Compilar e testar a aplicação localmente.
4. Instruir você a enviar as mudanças ao GitHub para atualizar o Render.

---

## 3. Instruções de Teste e Validação Pós-Deploy

Uma vez que o novo deploy estiver ativo no Render com o banco de dados limpo e configurado para as Quartas de Final:

1. **Acessar como Administrador**:
   - Faça login com o usuário padrão `admin@bolao.com` e a senha `admin123`.
   - Acesse o painel de **Administração** e verifique se as 8 seleções e os 4 confrontos estão listados corretamente.
2. **Convidar Participantes**:
   - Vá em **Meus Bolões**, clique em **Criar Novo Bolão** e copie o **Código de Convite**.
   - Compartilhe o código com os outros participantes.
3. **Simular o Fluxo de um Participante**:
   - Peça para os participantes entrarem no site, clicarem em **Entrar em um Bolão** e inserirem o código.
   - Cada participante deve dar seus palpites nos 4 jogos (ex: placar de França x Marrocos) antes do horário de início de cada partida.
4. **Finalizar Jogos e Conferir Ranking**:
   - Assim que um jogo real acontecer (ou para fins de teste agora), o administrador deve ir na aba **Administração**, digitar o placar do jogo, marcar como **Finalizada** e salvar.
   - A classificação do grupo (ranking de pontos) atualizará automaticamente no painel de cada participante.
