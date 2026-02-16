import React, { useMemo } from 'react'
import { useLanguage } from '@/i18n/LanguageContext'
import type { User, Event } from '@/services/apiService'

interface EventDetailProps {
  user: User
  events: Event[]
}

export default function EventDetail({ user, events }: EventDetailProps) {
  const { t } = useLanguage()
  
  const formatDate = (dateString: string) => {
    const date = new Date(dateString)
    return date.toLocaleDateString('pt-BR', { 
      month: 'short', 
      day: 'numeric', 
      year: 'numeric' 
    })
  }

  const formatTime = (dateString: string) => {
    const date = new Date(dateString)
    return date.toLocaleTimeString('pt-BR', { 
      hour: '2-digit', 
      minute: '2-digit' 
    })
  }

  const formatDuration = (start: string, end: string) => {
    const startDate = new Date(start)
    const endDate = new Date(end)
    const durationMs = endDate.getTime() - startDate.getTime()
    const hours = Math.floor(durationMs / (1000 * 60 * 60))
    const minutes = Math.floor((durationMs % (1000 * 60 * 60)) / (1000 * 60))
    
    if (hours > 0) {
      return `${hours}h ${minutes}m`
    }
    return `${minutes}m`
  }

  const isUpcoming = (dateString: string) => {
    return new Date(dateString) > new Date()
  }

  const isPast = (dateString: string) => {
    return new Date(dateString) < new Date()
  }

  const sortedEvents = useMemo(() => {
    return [...events].sort((a, b) => 
      new Date(a.startTime).getTime() - new Date(b.startTime).getTime()
    )
  }, [events])

  const upcomingEvents = useMemo(() => 
    sortedEvents.filter(e => isUpcoming(e.startTime)), 
    [sortedEvents]
  )

  const pastEvents = useMemo(() => 
    sortedEvents.filter(e => isPast(e.endTime)), 
    [sortedEvents]
  )

  return (
    <div>
      {/* User Info Header */}
      <div className="mb-6 pb-4 border-b border-gray-200">
        <div className="flex items-start justify-between">
          <div>
            <h3 className="text-lg font-semibold text-gray-900">{user.displayName}</h3>
            <p className="text-sm text-gray-600">{user.email}</p>
          </div>
          <div className="text-right">
            <p className="text-2xl font-bold text-blue-600">{events.length}</p>
            <p className="text-xs text-gray-500">{t.eventDetail.totalEvents}</p>
          </div>
        </div>
        
        <div className="flex gap-4 mt-4">
          <div className="flex items-center gap-2">
            <div className="w-3 h-3 rounded-full bg-green-500"></div>
            <span className="text-sm text-gray-600">{upcomingEvents.length} {t.eventDetail.upcoming}</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-3 h-3 rounded-full bg-gray-400"></div>
            <span className="text-sm text-gray-600">{pastEvents.length} {t.eventDetail.past}</span>
          </div>
        </div>
      </div>

      {events.length === 0 ? (
        <div className="text-center text-gray-500 py-12">
          <svg className="mx-auto h-12 w-12 text-gray-400 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
          </svg>
          <p className="font-medium">{t.eventDetail.noEventsScheduled}</p>
          <p className="text-sm mt-1">{t.eventDetail.noEventsMessage}</p>
        </div>
      ) : (
        <div className="space-y-4 max-h-[calc(100vh-28rem)] overflow-y-auto pr-2 custom-scrollbar">
          {sortedEvents.map((event) => {
            const upcoming = isUpcoming(event.startTime)
            const past = isPast(event.endTime)

            return (
              <div
                key={event.id}
                className={`p-4 border-l-4 rounded-lg shadow-sm transition-all hover:shadow-md ${
                  upcoming 
                    ? 'border-green-500 bg-green-50/50 hover:bg-green-50' 
                    : past 
                    ? 'border-gray-300 bg-gray-50/50 hover:bg-gray-50'
                    : 'border-blue-500 bg-blue-50/50 hover:bg-blue-50'
                }`}
              >
                {/* Event Header */}
                <div className="flex items-start justify-between mb-3">
                  <h4 className="font-semibold text-gray-900 flex-1 pr-2">{event.subject}</h4>
                  {upcoming && (
                    <span className="flex-shrink-0 px-2 py-1 bg-green-100 text-green-800 text-xs font-medium rounded-full">
                      {t.eventDetail.upcomingBadge}
                    </span>
                  )}
                </div>

                {/* Date and Time */}
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 mb-3 text-sm">
                  <div className="flex items-center gap-2 text-gray-700">
                    <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                    </svg>
                    <span>{formatDate(event.startTime)}</span>
                  </div>
                  <div className="flex items-center gap-2 text-gray-700">
                    <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                    <span>{formatTime(event.startTime)} - {formatTime(event.endTime)}</span>
                    <span className="text-gray-500">({formatDuration(event.startTime, event.endTime)})</span>
                  </div>
                </div>

                {/* Location and Organizer */}
                <div className="space-y-1 mb-3">
                  {event.location && (
                    <div className="flex items-center gap-2 text-sm text-gray-600">
                      <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                      </svg>
                      <span className="truncate">{event.location}</span>
                    </div>
                  )}

                  {event.organizer && (
                    <div className="flex items-center gap-2 text-sm text-gray-600">
                      <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                      </svg>
                      <span className="truncate">{event.organizer}</span>
                    </div>
                  )}
                </div>

                {/* Tags */}
                <div className="flex flex-wrap gap-2">
                  {event.isOnlineMeeting && (
                    <span className="inline-flex items-center gap-1 px-2 py-1 bg-blue-100 text-blue-800 text-xs font-medium rounded">
                      <svg className="w-3 h-3" fill="currentColor" viewBox="0 0 20 20">
                        <path d="M2 6a2 2 0 012-2h6a2 2 0 012 2v8a2 2 0 01-2 2H4a2 2 0 01-2-2V6zM14.553 7.106A1 1 0 0014 8v4a1 1 0 00.553.894l2 1A1 1 0 0018 13V7a1 1 0 00-1.447-.894l-2 1z" />
                      </svg>
                      {t.eventDetail.onlineMeeting}
                    </span>
                  )}
                </div>

                {/* Description Preview */}
                {event.bodyPreview && (
                  <p className="text-xs text-gray-600 mt-3 p-3 bg-white/70 rounded border border-gray-200 line-clamp-2">
                    {event.bodyPreview}
                  </p>
                )}
              </div>
            )
          })}
        </div>
      )}
    </div>
  )
}

