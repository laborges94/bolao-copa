# Diretrizes para Resolução: Implementação de Modo Escuro (Dark Mode) Dinâmico com MudBlazor

Este arquivo contém orientações para que o assistente de IA resolva a issue de Modo Escuro.

## Escopo do Problema
O site possui um visual verde/branco estático. Adicionar suporte ao Modo Escuro traz mais conforto visual aos usuários e melhora o apelo visual geral usando as facilidades do MudBlazor.

## Plano de Ação para o Assistente de IA
1. Crie uma classe de configurações de temas ou configure direto no layout os objetos `MudTheme` para o tema claro e escuro. Defina cores harmoniosas de paletas personalizadas (evite pretos absolutos, prefira tons de azul escuro ou cinza escuro para o background, e destaque com as cores verde e amarelo do futebol).
2. Abra o [MainLayout.razor](file:///c:/dev/Bolao/src/Bolao/Components/Layout/MainLayout.razor):
   - Adicione o `<MudThemeProvider @ref="_mudThemeProvider" @bind-IsDarkMode="_isDarkMode" Theme="_customTheme" />`.
   - Adicione um botão no cabeçalho ou menu lateral:
     ```razor
     <MudIconButton Icon="@(_isDarkMode ? Icons.Material.Filled.Brightness5 : Icons.Material.Filled.Brightness4)" Color="Color.Inherit" OnClick="@ToggleDarkMode" />
     ```
3. Implemente no `@code` a inversão da variável booliana `_isDarkMode`.
4. Salve o estado no `localStorage` por meio de JSInterop simples (ex: `await JSRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _isDarkMode.ToString())`) e leia no evento `OnAfterRenderAsync` com `firstRender = true` para aplicar o tema correto assim que a página carregar.
5. Verifique o contraste de todas as páginas e tabelas no modo escuro.
