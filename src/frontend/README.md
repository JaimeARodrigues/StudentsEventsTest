# Event Manager - Frontend

Modern React application built with Vite, TypeScript, and Tailwind CSS for managing student events.

## 🎯 Features

- ✅ **User Authentication** - Secure JWT-based login system
- ✅ **Student Management** - View and search through all students
- ✅ **Event Visualization** - Display student events with detailed information
- ✅ **Real-time Search** - Filter students by name or email
- ✅ **Data Synchronization** - Sync data from Microsoft Graph API
- ✅ **Responsive Design** - Works seamlessly on desktop, tablet, and mobile
- ✅ **Modern UI** - Clean and intuitive interface with Tailwind CSS
- ✅ **Loading States** - Visual feedback for all async operations
- ✅ **Error Handling** - User-friendly error messages

## 🛠️ Tech Stack

- **React 18** - Modern React with hooks
- **TypeScript** - Type-safe code
- **Vite** - Fast build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **Zustand** - Lightweight state management
- **Axios** - HTTP client for API calls
- **React Router** - Client-side routing
- **Vitest** - Fast unit testing framework
- **React Testing Library** - Testing utilities for React

## 📁 Project Structure

```
src/
├── components/          # Reusable UI components
│   ├── UserList.tsx    # Student list with search
│   ├── UserList.test.tsx # UserList tests
│   ├── EventDetail.tsx # Event details display
│   └── LanguageSelector.tsx # Language switcher
├── pages/              # Page components
│   ├── LoginPage.tsx   # Authentication page
│   └── DashboardPage.tsx # Main dashboard
├── services/           # API services
│   ├── api.ts          # Axios configuration
│   └── apiService.ts   # API endpoints
├── store/              # State management
│   └── authStore.ts    # Authentication state
├── i18n/               # Internationalization
│   ├── LanguageContext.tsx # Language provider
│   └── translations.ts # Translation strings
├── styles/             # Global styles
│   └── index.css       # Tailwind and custom styles
├── test/               # Test configuration
│   └── setup.ts        # Vitest setup file
├── App.tsx             # Root component
├── App.test.tsx        # App tests
└── main.tsx            # Application entry point
```

## 🚀 Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn
- Backend API running (see backend README)

### Installation

1. Navigate to frontend directory:
```bash
cd src/frontend
```

2. Install dependencies:
```bash
npm install
```

3. Configure API endpoint in `src/services/api.ts`:
```typescript
const API_BASE_URL = 'https://localhost:56057/' // Update if needed
```

4. Start development server:
```bash
npm run dev
```

The application will open at `http://localhost:5173`

## 📜 Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
- `npm run test` - Run tests (Vitest)
- `npm run test:ui` - Run tests with UI

## 🎨 Key Components

### LoginPage

Authentication page with:
- Email/password input
- Loading states
- Error handling
- Responsive design

### DashboardPage  

Main application page featuring:
- Student list with real-time search
- Event details panel
- Data synchronization button
- Responsive two-column layout

### UserList

Student list component with:
- Color-coded avatars
- Interactive selection
- Search filtering
- Empty state handling

### EventDetail

Event details component showing:
- Event timeline (upcoming/past)
- Date and time formatting
- Location and organizer info
- Online meeting indicator
- Event duration calculation

## 🔐 Authentication

The app uses JWT Bearer tokens for authentication:
- Token stored in localStorage
- Automatic token injection in API calls
- Protected routes with authentication check
- Auto-redirect to login if not authenticated

## 📱 Responsive Design

The interface adapts to different screen sizes:
- **Mobile** (< 640px): Single column, stacked layout
- **Tablet** (640px - 1024px): Optimized spacing and touch targets
- **Desktop** (> 1024px): Full two-column layout

## 🔄 State Management

Uses Zustand for lightweight state management:
- Authentication state
- User information
- Token management
- Persistent storage (localStorage)

## 📊 API Integration

All API calls go through centralized services:
- `authService` - Authentication
- `userService` - User operations
- `eventService` - Event operations
- `syncService` - Data synchronization

## 🧪 Testing

O projeto possui testes unitários completos para os componentes principais usando Vitest e React Testing Library.

### Executar Testes

Rodar todos os testes:
```bash
npm run test
```

Interface interativa de testes:
```bash
npm run test:ui
```

Rodar testes em modo CI (sem watch):
```bash
npm run test -- --run
```

### Cobertura de Testes

#### App.test.tsx (3 testes)
Testa o componente principal da aplicação:
- ✅ Renderização do título "Event Manager"
- ✅ Exibição da mensagem "Loading application..."
- ✅ Verificação das classes de estilo corretas

#### UserList.test.tsx (4 testes)
Testa a lista de estudantes:
- ✅ Exibição de ícone SVG quando lista vazia
- ✅ Renderização de nomes e emails dos usuários
- ✅ Callback ao clicar em um usuário
- ✅ Destaque visual do usuário selecionado

#### LanguageSelector.test.tsx (5 testes)
Testa o seletor de idioma:
- ✅ Renderização do botão com aria-label
- ✅ Exibição da bandeira do idioma atual (🇧🇷/🇺🇸)
- ✅ Abertura do dropdown ao clicar
- ✅ Troca de idioma ao selecionar opção
- ✅ Fechamento automático após seleção

### Stack de Testes

- **Vitest** (v0.34.6) - Test runner rápido e compatível com Vite
- **@testing-library/react** - Utilitários para testar componentes React
- **@testing-library/jest-dom** - Matchers customizados para asserções DOM
- **@testing-library/user-event** - Simulação realista de interações
- **jsdom** - Implementação de DOM para ambiente Node.js

### Configuração

Os testes são configurados em:
- `vite.config.ts` - Configuração do Vitest
- `src/test/setup.ts` - Setup global dos testes

### Boas Práticas

✅ Testes focam no comportamento do usuário
✅ Componentes são envolvidos com providers necessários (LanguageProvider)
✅ Uso de queries semânticas (getByRole, getByText)
✅ Simulação realista de eventos do usuário
✅ Cleanup automático após cada teste

## 🚀 Deployment

Build for production:
```bash
npm run build
```

The `dist` folder contains production-ready files.

### Deploy Options

- **Vercel** - `vercel --prod`
- **Netlify** - Drag & drop `dist` folder
- **Azure Static Web Apps**
- **GitHub Pages**

---

Built with ❤️ using React + TypeScript + Tailwind CSS
