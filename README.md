# Sistema de Controle de Ponto

Sistema completo para controle de ponto de funcionários, desenvolvido com .NET Web API no backend e Next.js no frontend.

## 🚀 Tecnologias Utilizadas

### Backend
- .NET 9.0 Web API
- Entity Framework Core
- SQLite Database
- Swagger/OpenAPI
- CORS habilitado

### Frontend
- Next.js 15 com TypeScript
- React com Hooks
- TailwindCSS
- Axios
- Lucide React (ícones)

## 📋 Funcionalidades

- ✅ **Cadastro de Funcionários**: CRUD completo com validação
- ✅ **Marcação de Ponto**: Entrada e saída com validação de horários
- ✅ **Relatórios**: Filtros, estatísticas e exportação para CSV
- ✅ **Interface Responsiva**: Design moderno e intuitivo
- ✅ **API RESTful**: Documentação automática com Swagger

## 🛠️ Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- Node.js 18+ e npm

### Backend (API)
```bash
cd backend/ControlePonto
dotnet restore
dotnet run
```
A API estará disponível em: `http://localhost:5057`

### Frontend
```bash
cd frontend/controle-ponto-frontend
npm install
npm run dev
```
A aplicação estará disponível em: `http://localhost:3000`

### Configuração
1. Copie o arquivo `.env.example` para `.env.local` no frontend
2. Configure a URL da API no arquivo `.env.local`:
```
NEXT_PUBLIC_API_URL=http://localhost:5057/api
```

## 📖 Documentação da API

Com o backend rodando, acesse:
- Swagger UI: `http://localhost:5057/swagger`

### Endpoints Principais

#### Funcionários
- `GET /api/funcionarios` - Lista todos os funcionários
- `POST /api/funcionarios` - Cria novo funcionário
- `PUT /api/funcionarios/{id}` - Atualiza funcionário
- `DELETE /api/funcionarios/{id}` - Remove funcionário

#### Pontos
- `GET /api/pontos` - Lista registros de ponto (com filtros)
- `GET /api/pontos/por-cargo?cargo=DEV` - Lista registros de ponto filtrando por cargo
- `POST /api/pontos` - Marca entrada
- `PUT /api/pontos/{id}/saida` - Marca saída

## 🗂️ Estrutura do Projeto

```
├── backend/
│   └── ControlePonto/           # API .NET
│       ├── Controllers/         # Controllers da API
│       ├── Data/               # Contexto do Entity Framework
│       ├── DTOs/               # Data Transfer Objects
│       ├── Models/             # Modelos de dados
│       └── Program.cs          # Configuração da aplicação
├── frontend/
│   └── controle-ponto-frontend/ # Aplicação Next.js
│       ├── src/
│       │   ├── app/            # Páginas da aplicação
│       │   ├── components/     # Componentes reutilizáveis
│       │   ├── services/       # Serviços de API
│       │   └── types/          # Tipos TypeScript
│       └── package.json
└── README.md
```

## 📱 Páginas da Aplicação

### 🏠 Página Principal - Marcar Ponto
- Seleção de funcionário
- Botões para marcar entrada/saída
- Validação de horários

### 👥 Funcionários
- Lista de funcionários cadastrados
- Formulário de cadastro/edição
- Exclusão com confirmação

### 📊 Relatórios
- Lista de todos os registros de ponto
- Filtros por funcionário e período
- Estatísticas (horas trabalhadas, média)
- Exportação para CSV

## 🎯 Dados de Exemplo

O sistema vem com dados pré-cadastrados para teste:
- João Silva (Desenvolvedor)
- Maria Santos (Analista de Sistemas)
- Pedro Oliveira (Gerente de Projetos)

## 🔧 Scripts Úteis

### Backend
```bash
# Restaurar dependências
dotnet restore

# Executar aplicação
dotnet run

# Build para produção
dotnet build --configuration Release
```

### Frontend
```bash
# Instalar dependências
npm install

# Executar em desenvolvimento
npm run dev

# Build para produção
npm run build

# Executar versão de produção
npm start

# Verificar lint
npm run lint
```

## 📄 Licença

Este projeto está sob a licença MIT.

## 👨‍💻 Desenvolvedor

Desenvolvido para demonstração de habilidades em desenvolvimento full-stack com .NET e React/Next.js.
