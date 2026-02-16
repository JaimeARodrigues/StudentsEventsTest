import React from 'react'
import { useLanguage } from '@/i18n/LanguageContext'
import type { User } from '@/services/apiService'

interface UserListProps {
  users: User[]
  selectedUserId: string | null
  onSelectUser: (userId: string) => void
}

export default function UserList({ users, selectedUserId, onSelectUser }: UserListProps) {
  const { t } = useLanguage()
  
  if (users.length === 0) {
    return (
      <div className="text-center text-gray-500 py-12">
        <svg className="mx-auto h-12 w-12 text-gray-400 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
        </svg>
        <p className="font-medium">{t.userList.noUsersFound}</p>
        <p className="text-sm mt-1">{t.userList.noUsersMessage}</p>
      </div>
    )
  }

  const getInitials = (name: string) => {
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2)
  }

  const getAvatarColor = (id: string) => {
    const colors = [
      'bg-blue-500',
      'bg-green-500',
      'bg-purple-500',
      'bg-pink-500',
      'bg-indigo-500',
      'bg-yellow-500',
      'bg-red-500',
      'bg-teal-500',
    ]
    const index = id.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0) % colors.length
    return colors[index]
  }

  return (
    <div className="space-y-2 max-h-[calc(100vh-24rem)] overflow-y-auto p-1 pr-3 custom-scrollbar">
      {users.map((user) => (
        <button
          key={user.id}
          onClick={() => onSelectUser(user.id)}
          className={`w-full text-left px-4 py-3 rounded-lg border-2 transition-all duration-200 ${
            selectedUserId === user.id
              ? 'border-blue-500 bg-blue-50 shadow-md scale-[1.01]'
              : 'border-gray-200 bg-white hover:border-gray-300 hover:shadow-sm'
          }`}
        >
          <div className="flex items-center gap-3">
            {/* Avatar */}
            <div className={`flex-shrink-0 w-10 h-10 rounded-full ${getAvatarColor(user.id)} flex items-center justify-center text-white font-semibold text-sm`}>
              {getInitials(user.displayName)}
            </div>
            
            {/* User Info */}
            <div className="flex-1 min-w-0">
              <div className="font-semibold text-gray-900 truncate">{user.displayName}</div>
              <div className="text-sm text-gray-600 truncate">{user.email}</div>
            </div>

            {/* Selected Indicator */}
            {selectedUserId === user.id && (
              <svg className="w-5 h-5 text-blue-500 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
              </svg>
            )}
          </div>
        </button>
      ))}
    </div>
  )
}
