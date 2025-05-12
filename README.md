# BookSphere

**BookSphere** é uma biblioteca digital projetada para oferecer uma experiência intuitiva e rica, permitindo que os usuários explorem, gerenciem e compartilhem livros de forma eficiente. Ideal para leitores e administradores.

---

## 📚 Funcionalidades

- **📖 Gerenciamento de Livros**: Adicione, edite e remova livros do catálogo.
- **⭐ Favoritos**: Marque livros como favoritos para acesso rápido.
- **📂 Categorias**: Organize livros por categorias personalizadas.
- **👤 Perfil de Usuário**: Gerencie informações pessoais e preferências.
- **🔒 Sistema de Autenticação**: Login e registro de usuários com segurança.

---

## 🛠️ Tecnologias Utilizadas

### Backend
- **Linguagem:** C#
- **Framework:** ASP.NET Core
- **Banco de Dados:** Entity Framework Core com suporte a SQL Server
- **Ferramentas Adicionais:**
    - **Identity**: Gerenciamento de autenticação e autorização.
    - **AutoMapper**: Mapeamento de objetos simplificado.
    - **FluentValidation**: Validação robusta de dados.

### Frontend
- **Linguagens:** HTML, CSS, JavaScript
- **Framework:** Razor Pages
- **Bibliotecas:** Bootstrap para estilização e responsividade.

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- .NET SDK 6.0 ou superior
- SQL Server
- Visual Studio ou Visual Studio Code
### Passos para Configuração
1. **Configurar o Banco de Dados**:
    - Atualize a string de conexão no arquivo `appsettings.json`.

2. **Criar o Banco de Dados (se não existir)**:
    - Execute o comando abaixo para criar o banco de dados:
    ```bash
    dotnet ef database update
    ```

3. **Iniciar o Projeto**:
    ```bash
    dotnet run
    ```

4. **Acessar a Aplicação**:
    - Abra o navegador e acesse: [http://localhost:5000]

---