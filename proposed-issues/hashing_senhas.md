# Diretrizes para Resolução: Fortalecimento do Hashing de Senhas (BCrypt ou Identity)

Este arquivo contém orientações para que o assistente de IA resolva a issue de hashing de senhas.

## Escopo do Problema
O arquivo [PasswordHasher.cs](file:///c:/dev/Bolao/src/Bolao/Helpers/PasswordHasher.cs) atual executa apenas um SHA256 direto sobre a senha em string, retornando o valor em formato hexadecimal de letras minúsculas. Isso falha em padrões modernos de segurança de dados de usuários por não possuir salt.

## Plano de Ação para o Assistente de IA
1. Instale o pacote `BCrypt.Net-Next` no projeto principal [Bolao.csproj](file:///c:/dev/Bolao/src/Bolao/Bolao.csproj).
2. Modifique a classe estática [PasswordHasher.cs](file:///c:/dev/Bolao/src/Bolao/Helpers/PasswordHasher.cs):
   - O método `Hash` deve chamar `BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 11)`.
   - Adicione um método `Verify(string password, string hashedPassword)` que chama `BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword)`.
3. Ajuste os arquivos do fluxo de cadastro e login:
   - No [Register.razor](file:///c:/dev/Bolao/src/Bolao/Components/Pages/Register.razor), salve o hash robusto gerado.
   - No [Program.cs](file:///c:/dev/Bolao/src/Bolao/Program.cs#L84-L85) (Minimal API de login), o método não pode comparar strings hexadecimais diretamente (`u.Senha == hashedPassword`). Deve ler o usuário por email e então aplicar `PasswordHasher.Verify(password, user.Senha)`.
4. Atualize [SeedData.cs](file:///c:/dev/Bolao/src/Bolao/Data/SeedData.cs#L129) para que a senha do administrador seja hashada usando o novo padrão do BCrypt.
5. Atualize os testes unitários em [PasswordHasherTests.cs](file:///c:/dev/Bolao/tests/Bolao.Tests/PasswordHasherTests.cs) para se adequarem aos novos métodos.
