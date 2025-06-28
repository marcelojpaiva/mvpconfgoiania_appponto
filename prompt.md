Crie uma aplicação fullstack para controle de ponto de funcionários com as seguintes características:

## 🎯 Descrição Geral:
O sistema será composto por:
- Um **backend em .NET 6 ou superior (C#)** com Web API RESTful.
- Um **frontend em Node.js usando Next.js**, com páginas básicas de cadastro e marcação de ponto.

## 🧱 Funcionalidades básicas:
1. **Cadastro de Funcionário**
   - Nome
   - E-mail
   - Cargo

2. **Registro de Ponto**
   - Selecionar funcionário
   - Registrar data e hora de entrada e saída

3. **Listagem de pontos registrados**
   - Filtro por data e por funcionário

## 🖥️ Backend (.NET C# API):
- Use ASP.NET Core Web API.
- Use Entity Framework Core com SQLite ou InMemory.
- Crie modelos, contextos, controllers e DTOs.
- Endpoints principais:
   - POST /funcionarios
   - GET /funcionarios
   - POST /pontos
   - GET /pontos?funcionarioId=1&data=2025-06-27

## 🌐 Frontend (Next.js com React):
- Página inicial com formulário para marcar ponto
- Página para cadastro de funcionários
- Página de listagem dos registros
- Consuma a API .NET no frontend usando fetch ou axios
- Interface simples e funcional (pode usar TailwindCSS ou outro framework)

## ⚙️ Extras (opcional):
- Validação básica de formulário
- Controle de CORS no backend para permitir acesso local do frontend