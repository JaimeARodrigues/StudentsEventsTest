import { create } from 'zustand'

interface User {
  id: string
  email: string
  displayName: string
  givenName?: string
  surname?: string
}

interface AuthStore {
  token: string | null
  user: User | null
  login: (token: string, user: User) => void
  logout: () => void
  isAuthenticated: () => boolean
}

export const useAuthStore = create<AuthStore>((set, get) => ({
  token: localStorage.getItem('token'),
  user: localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')!) : null,
  
  login: (token: string, user: User) => {
    localStorage.setItem('token', token)
    localStorage.setItem('user', JSON.stringify(user))
    set({ token, user })
  },
  
  logout: () => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    set({ token: null, user: null })
  },
  
  isAuthenticated: () => {
    return get().token !== null
  }
}))
