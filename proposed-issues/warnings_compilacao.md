# Diretrizes para Resolução: Resolução de Avisos (Warnings) de Compilação e Analisadores

Este arquivo contém orientações para que o assistente de IA resolva a issue de avisos de compilação.

## Escopo do Problema
Compilar a solução gera múltiplos alertas (Warnings) no C# e analisador do MudBlazor, poluindo o console e escondendo potenciais bugs.

## Avisos para Resolver
1. **Chamada Obsoleta**: [Register.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/Register.razor#L70) usa `MudForm.Validate()` que foi depreciado em versões novas do MudBlazor.
   - *Ação*: Mude para `await _form.Validate()` ou `_form.ValidateAsync()`.
2. **Potencial NullReferenceException (CS8602)**:
   - Em [MeusBoloes.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/MeusBoloes.razor#L149) (desreferência de `b`).
   - Em [BolaoDetail.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/BolaoDetail.razor#L453) (desreferência de `p.Partida.Finalizada`, `p.Partida.TimeCasa`, etc.).
   - *Ação*: Use operadores de navegação segura (`?.`) ou verificações explícitas de nulo.
3. **Campo Não Utilizado (CS0414)**:
   - Campo privado `_loading` em [Admin.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/Admin.razor#L177).
   - *Ação*: Remova a variável não utilizada.
4. **Atributos Inválidos no MudBlazor (MUD0002)**:
   - Atributo `Name` em `MudTextField` em [Login.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/Login.razor).
   - Atributo `Align` em `MudTh` e `MudTd` em [BolaoDetail.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/BolaoDetail.razor).
   - *Ação*: Remova `Name` (use `Label` ou `Id` se necessário) e para `Align`, configure o alinhamento de texto via classes CSS (ex: `class="text-center"`, `class="text-right"`) ou use o atributo padrão correto do MudBlazor (como `Style="text-align: center"`).

## Plano de Ação para o Assistente de IA
1. Abra os arquivos informados.
2. Analise cada linha reportada pelo compilador/analisador.
3. Aplique as modificações necessárias sem alterar a regra de negócio.
4. Rode `dotnet build` localmente para atestar que os avisos sumiram.
