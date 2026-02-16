import React, { useEffect, useState, useMemo } from 'react'
import { useNavigate } from 'react-router-dom'
import { userService, syncService, Event } from '@/services/apiService'
import { useAuthStore } from '@/store/authStore'
import { useLanguage } from '@/i18n/LanguageContext'
import UserList from '@/components/UserList'
import EventDetail from '@/components/EventDetail'
import LanguageSelector from '@/components/LanguageSelector'
import type { User } from '@/services/apiService'

export default function DashboardPage() {
  const navigate = useNavigate()
  const { user, logout } = useAuthStore()
  const { t } = useLanguage()
  const [users, setUsers] = useState<User[]>([])
  const [selectedUserId, setSelectedUserId] = useState<string | null>(null)
  const [selectedUser, setSelectedUser] = useState<User | null>(null)
  const [events, setEvents] = useState<Event[]>([])
  const [loading, setLoading] = useState(true)
  const [loadingEvents, setLoadingEvents] = useState(false)
  const [syncing, setSyncing] = useState(false)
  const [error, setError] = useState('')
  const [searchTerm, setSearchTerm] = useState('')
  const [successMessage, setSuccessMessage] = useState('')

  useEffect(() => {
    if (!user) {
      navigate('/login')
    }
  }, [user, navigate])

  useEffect(() => {
    loadUsers()
  }, [])

  const loadUsers = async () => {
    try {
      setLoading(true)
      const data = await userService.getAllUsers()
      setUsers(data)
      setError('')
    } catch (err) {
      setError(t.dashboard.loadUsersError)
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  const handleSelectUser = async (userId: string) => {
    try {
      setLoadingEvents(true)
      setSelectedUserId(userId)
      setError('')
      const result = await userService.getUserWithEvents(userId)
      setSelectedUser(result.user)
      setEvents(result.events || [])
    } catch (err) {
      setError(t.dashboard.loadEventsError)
      console.error(err)
    } finally {
      setLoadingEvents(false)
    }
  }

  const handleSync = async () => {
    try {
      setSyncing(true)
      setError('')
      setSuccessMessage('')
      
      await syncService.syncUsers()
      await syncService.syncEvents()
      
      setSuccessMessage(t.dashboard.syncSuccess)
      
      // Reload users after sync
      await loadUsers()
      
      // Reload selected user events if any
      if (selectedUserId) {
        await handleSelectUser(selectedUserId)
      }
      
      setTimeout(() => setSuccessMessage(''), 3000)
    } catch (err) {
      setError(t.dashboard.syncError)
      console.error(err)
    } finally {
      setSyncing(false)
    }
  }

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  // Filter users based on search term
  const filteredUsers = useMemo(() => {
    if (!searchTerm.trim()) return users
    
    const search = searchTerm.toLowerCase()
    return users.filter(u => 
      u.displayName?.toLowerCase().includes(search) ||
      u.email?.toLowerCase().includes(search) ||
      u.givenName?.toLowerCase().includes(search) ||
      u.surname?.toLowerCase().includes(search)
    )
  }, [users, searchTerm])

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white border-b border-gray-200 sticky top-0 z-10 shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">{t.dashboard.title}</h1>
              <p className="text-xs text-gray-500 mt-1">{t.dashboard.subtitle}</p>
            </div>
            <div className="flex items-center gap-3 w-full sm:w-auto">
              <button
                onClick={handleSync}
                disabled={syncing}
                className="px-3 py-2 text-sm font-medium text-blue-700 bg-blue-50 hover:bg-blue-100 rounded-lg transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
              >
                <svg className={`w-4 h-4 ${syncing ? 'animate-spin' : ''}`} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                </svg>
                {syncing ? t.dashboard.syncing : t.dashboard.sync}
              </button>
              <LanguageSelector />
              <span className="text-sm text-gray-600 hidden sm:block">{user?.displayName || user?.email}</span>
              <button
                onClick={handleLogout}
                className="px-4 py-2 text-sm font-medium text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-lg transition"
              >
                {t.dashboard.logout}
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8">
        {error && (
          <div className="mb-4 p-4 bg-red-50 border border-red-200 text-red-700 rounded-lg flex items-center gap-2">
            <svg className="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
            <span>{error}</span>
          </div>
        )}

        {successMessage && (
          <div className="mb-4 p-4 bg-green-50 border border-green-200 text-green-700 rounded-lg flex items-center gap-2">
            <svg className="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
            </svg>
            <span>{successMessage}</span>
          </div>
        )}

        <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
          {/* Users List */}
          <div className="lg:col-span-5 bg-white rounded-lg shadow-md overflow-hidden">
            <div className="p-4 sm:p-6 border-b border-gray-200">
              <div className="flex items-center justify-between mb-4">
                <h2 className="text-xl font-bold text-gray-900">{t.userList.title}</h2>
                <span className="text-sm text-gray-500 bg-gray-100 px-3 py-1 rounded-full">
                  {filteredUsers.length} {filteredUsers.length === 1 ? t.userList.student : t.userList.students}
                </span>
              </div>
              
              {/* Search Input */}
              <div className="relative">
                <svg className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
                <input
                  type="text"
                  placeholder={t.userList.searchPlaceholder}
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none text-sm"
                />
                {searchTerm && (
                  <button
                    onClick={() => setSearchTerm('')}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                  >
                    <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                      <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                    </svg>
                  </button>
                )}
              </div>
            </div>
            
            <div className="p-4 sm:p-6">
              {loading ? (
                <div className="space-y-3">
                  {[...Array(5)].map((_, i) => (
                    <div key={i} className="h-20 bg-gray-200 rounded-lg animate-pulse" />
                  ))}
                </div>
              ) : (
                <UserList 
                  users={filteredUsers} 
                  selectedUserId={selectedUserId}
                  onSelectUser={handleSelectUser}
                />
              )}
            </div>
          </div>

          {/* Events Detail */}
          <div className="lg:col-span-7 bg-white rounded-lg shadow-md overflow-hidden">
            <div className="p-4 sm:p-6 border-b border-gray-200">
              <h2 className="text-xl font-bold text-gray-900">{t.eventDetail.title}</h2>
              {selectedUser && (
                <p className="text-sm text-gray-500 mt-1">{t.eventDetail.viewingEventsFor} {selectedUser.displayName}</p>
              )}
            </div>
            
            <div className="p-4 sm:p-6">
              {loadingEvents ? (
                <div className="space-y-4">
                  {[...Array(3)].map((_, i) => (
                    <div key={i} className="h-32 bg-gray-200 rounded-lg animate-pulse" />
                  ))}
                </div>
              ) : selectedUser ? (
                <EventDetail user={selectedUser} events={events} />
              ) : (
                <div className="text-center text-gray-500 py-16">
                  <svg className="mx-auto h-12 w-12 text-gray-400 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  <p className="text-lg font-medium">{t.eventDetail.selectStudent}</p>
                  <p className="text-sm mt-1">{t.eventDetail.selectStudentMessage}</p>
                </div>
              )}
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
