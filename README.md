# Event Manager - Provinha

Sistema fullstack para gerenciamento de eventos em instituições educacionais, construído com React, TypeScript, .NET 8 e SQL Server.

---

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Recursos Implementados](#recursos-implementados)
- [Pré-requisitos](#pré-requisitos)
- [Como Rodar o Projeto](#como-rodar-o-projeto)
- [Como Rodar os Testes](#como-rodar-os-testes)
- [Estrutura do Projeto](#estrutura-do-projeto)

---

## 🎯 Visão Geral

O **Event Manager** é uma aplicação web fullstack que permite gerenciar eventos e usuários em instituições educacionais. A aplicação integra-se com Microsoft Graph API para sincronização de dados e oferece autenticação segura com JWT.

---

## ✨ Recursos Implementados

### Backend (.NET 8)

- ✅ **API RESTful** - Endpoints seguindo padrões REST
- ✅ **Autenticação JWT** - Sistema de login seguro
- ✅ **Entity Framework Core 8.0** - ORM para acesso a dados
- ✅ **SQL Server** - Banco de dados relacional
- ✅ **Migrations** - Versionamento de schema do banco
- ✅ **Integração Microsoft Graph** - Sincronização com Microsoft 365
- ✅ **Hangfire** - Background jobs para processamento assíncrono
- ✅ **Swagger** - Documentação automática de API
- ✅ **Testes com xUnit** - Testes unitários

### Frontend (React 18 + TypeScript)

- ✅ **Autenticação JWT** - Sistema de login seguro com tokens
- ✅ **Gerenciamento de Estudantes** - Visualizar e pesquisar todos os estudantes
- ✅ **Visualização de Eventos** - Exibir eventos de estudantes com informações detalhadas
- ✅ **Busca em Tempo Real** - Filtrar estudantes por nome ou email
- ✅ **Sincronização de Dados** - Integração com Microsoft Graph API
- ✅ **Design Responsivo** - Funciona em desktop, tablet e mobile
- ✅ **Interface Moderna** - UI limpa e intuitiva com Tailwind CSS
- ✅ **Estados de Carregamento** - Feedback visual para operações assíncronas
- ✅ **Tratamento de Erros** - Mensagens de erro amigáveis
- ✅ **Testes com Vitest** - Testes unitários para componentes

---

## 📋 Pré-requisitos

### Backend
- **.NET 8 SDK** ou superior
- **SQL Server** 2019 ou superior (ou usar LocalDB)
- **Node.js e npm** (apenas para gerenciar scripts no package.json raiz)

### Frontend
- **Node.js** 18 ou superior
- **npm** 9 ou superior

---

## 🚀 Como Rodar o Projeto

### Opção 1: Rodando Backend e Frontend Separadamente (Recomendado)

#### Setup Inicial

Na raiz do projeto, instale as dependências do frontend:

```bash
npm run frontend:install
```

#### Rodar o Backend

1. Restaure as dependências:
```bash
npm run backend:restore
```

2. Configure o banco de dados em `src/backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EventManagerDb;Trusted_Connection=true;"
  }
}
```

3. Aplique as migrations:
```bash
npm run backend:migrate
```

4. Inicie o backend:
```bash
npm run backend:run
```

O backend estará disponível em `https://localhost:7000` (ou conforme configurado).

#### Rodar o Frontend

Em outro terminal, execute:

```bash
npm run frontend:dev
```

O frontend estará disponível em `http://localhost:5173`

### Opção 2: Compilar para Produção

#### Build Backend
```bash
npm run backend:build
```

#### Build Frontend
```bash
npm run frontend:build
```

---

## 🧪 Como Rodar os Testes

### Testes Backend

Execute os testes unitários do backend:

```bash
npm run backend:test
```

Os testes utilizam **xUnit** e estão localizados em `src/backend/Tests/`

### Testes Frontend

#### Rodar testes com Vitest

```bash
npm run frontend:test
```

#### Rodar testes com interface visual

```bash
npm run frontend:test:ui
```

Os testes utilizam **Vitest** com **React Testing Library** e estão localizados em `src/frontend/src/`

#### Testes Implementados

- **App.test.tsx** - Testes do componente principal (3 testes)
  - Renderização do título "Event Manager"
  - Exibição da mensagem de carregamento
  - Verificação de classes CSS aplicadas

- **UserList.test.tsx** - Testes da lista de usuários (4 testes)
  - Exibição de mensagem quando não há usuários
  - Renderização da lista de usuários
  - Interação ao clicar em um usuário
  - Destaque visual do usuário selecionado

- **LanguageSelector.test.tsx** - Testes do seletor de idioma (5 testes)
  - Renderização do botão de seleção
  - Exibição da bandeira do idioma atual
  - Abertura do dropdown ao clicar
  - Mudança de idioma ao selecionar opção
  - Fechamento do dropdown após seleção

#### Tecnologias de Teste

- **Vitest** - Framework de testes rápido e moderno
- **React Testing Library** - Testes focados no comportamento do usuário
- **jsdom** - Ambiente de DOM para testes
- **@testing-library/user-event** - Simulação de interações do usuário
- **@testing-library/jest-dom** - Matchers customizados para DOM

---

## 📁 Estrutura do Projeto

```
Provinha/
├── src/
│   ├── backend/                    # API .NET 8
│   │   ├── API/                    # Controllers
│   │   ├── Application/            # Services e DTOs
│   │   ├── Domain/                 # Entities e Interfaces
│   │   ├── Infrastructure/         # Database e Repositories
│   │   ├── Tests/                  # Testes unitários
│   │   ├── EventManager.API.csproj # Projeto principal
│   │   └── Program.cs              # Configuração
│   │
│   └── frontend/                   # Aplicação React
│       ├── src/
│       │   ├── components/         # Componentes reutilizáveis
│       │   ├── pages/              # Páginas da aplicação
│       │   ├── services/           # Serviços de API
│       │   ├── store/              # Gerenciamento de estado (Zustand)
│       │   ├── styles/             # Estilos globais
│       │   ├── App.tsx             # Componente raiz
│       │   └── main.tsx            # Ponto de entrada
│       ├── package.json            # Dependências npm
│       ├── vite.config.ts          # Configuração Vite
│       ├── tsconfig.json           # Configuração TypeScript
│       └── tailwind.config.js      # Configuração Tailwind CSS
│
├── package.json                    # Scripts do projeto raiz
├── .gitignore
└── README.md                       # Este arquivo
```

---

## 🛠️ Stack Tecnológico

### Backend
- **.NET 8** - Framework web moderno
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Banco de dados
- **JWT Bearer** - Autenticação
- **Microsoft Graph** - Integração com Microsoft 365
- **Hangfire** - Job scheduler
- **xUnit** - Framework de testes
- **Swagger** - Documentação de API

### Frontend
- **React 18** - Biblioteca UI
- **TypeScript** - Tipagem estática
- **Vite** - Build tool e dev server
- **Tailwind CSS** - Estilização
- **Zustand** - State management
- **Axios** - HTTP client
- **React Router** - Roteamento
- **Vitest** - Framework de testes

---

## 📝 Variáveis de Ambiente

### Frontend

Crie um arquivo `.env.development` em `src/frontend/`:

```
VITE_API_URL=https://localhost:7000/api
```

### Backend

Configure em `src/backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EventManagerDb;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "ExpirationMinutes": 60
  }
}
```

---

## 📜 Scripts Disponíveis

O projeto possui scripts npm na raiz para facilitar o desenvolvimento:

### Backend
- `npm run backend:restore` - Restaurar dependências do backend
- `npm run backend:migrate` - Aplicar migrations do banco de dados
- `npm run backend:run` - Executar o backend em modo desenvolvimento
- `npm run backend:build` - Compilar o backend para produção
- `npm run backend:test` - Executar testes unitários do backend

### Frontend
- `npm run frontend:install` - Instalar dependências do frontend
- `npm run frontend:dev` - Executar o frontend em modo desenvolvimento
- `npm run frontend:build` - Compilar o frontend para produção
- `npm run frontend:test` - Executar testes unitários do frontend
- `npm run frontend:test:ui` - Executar testes com interface visual

### Geral
- `npm run dev` - Exibir instruções para iniciar backend e frontend

---

## 🤝 Contribuindo

Para contribuir com o projeto, faça um fork, crie uma branch com sua feature (`git checkout -b feature/AmazingFeature`) e envie um pull request.

---

## 📄 Licença

Este projeto está licenciado sob a License MIT - veja o arquivo LICENSE para mais detalhes.

---

## 👥 Autores

Desenvolvido por Jaime Rodrigues

---

**Última atualização:** Fevereiro 2026
