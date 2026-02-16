import api from './api'

export interface User {
  id: string
  email: string
  displayName: string
  givenName?: string
  surname?: string
}

export interface Event {
  id: string
  subject: string
  bodyPreview?: string
  startTime: string
  endTime: string
  location?: string
  organizer?: string
  isOnlineMeeting: boolean
}

export const authService = {
  login: async (email: string, password: string) => {
    const response = await api.post('/api/auth/login', { email, password })
    return response.data
  },
}

export const userService = {
  getAllUsers: async () => {
    const response = await api.get('/api/users')
    return response.data as User[]
  },

  getUserById: async (id: string) => {
    const response = await api.get(`/api/users/${id}`)
    return response.data as User
  },

  getUserWithEvents: async (id: string) => {
    const response = await api.get(`/api/users/${id}/events`)
    return response.data
  },
}

export const eventService = {
  getByUserId: async (userId: string) => {
    const response = await api.get(`/api/events/user/${userId}`)
    return response.data as Event[]
  },

  getById: async (id: string) => {
    const response = await api.get(`/api/events/${id}`)
    return response.data as Event
  },
}

export const syncService = {
  syncUsers: async () => {
    const response = await api.post('/api/sync/users')
    return response.data
  },

  syncEvents: async () => {
    const response = await api.post('/api/sync/events')
    return response.data
  },

  scheduleSync: async () => {
    const response = await api.post('/api/sync/schedule')
    return response.data
  },
}
