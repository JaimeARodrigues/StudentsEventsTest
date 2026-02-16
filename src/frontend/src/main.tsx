import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { useAuthStore } from './store/authStore'
import { LanguageProvider } from './i18n/LanguageContext'
import App from './App'
import LoginPage from './pages/LoginPage'
import DashboardPage from './pages/DashboardPage'
import '@/styles/index.css'

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated())

  if (!isAuthenticated) {
    return <Navigate to="/login" />
  }

  return <>{children}</>
}

function AppRouter() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          }
        />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </Router>
  )
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <LanguageProvider>
      <AppRouter />
    </LanguageProvider>
  </React.StrictMode>,
)
