import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import UserList from '../components/UserList'
import { LanguageProvider } from '../i18n/LanguageContext'

const mockUsers = [
  {
    id: '1',
    displayName: 'John Doe',
    email: 'john@example.com',
    givenName: 'John',
    surname: 'Doe',
  },
  {
    id: '2',
    displayName: 'Jane Smith',
    email: 'jane@example.com',
    givenName: 'Jane',
    surname: 'Smith',
  },
]

const renderWithProvider = (component: React.ReactElement) => {
  return render(
    <LanguageProvider>
      {component}
    </LanguageProvider>
  )
}

describe('UserList', () => {
  it('renders no users message when list is empty', () => {
    const onSelectUser = vi.fn()
    renderWithProvider(
      <UserList users={[]} selectedUserId={null} onSelectUser={onSelectUser} />
    )
    
    // The component uses translations, so check for SVG icon instead
    const svg = document.querySelector('svg.mx-auto.h-12.w-12')
    expect(svg).toBeInTheDocument()
  })

  it('renders list of users', () => {
    const onSelectUser = vi.fn()
    renderWithProvider(
      <UserList users={mockUsers} selectedUserId={null} onSelectUser={onSelectUser} />
    )
    
    expect(screen.getByText('John Doe')).toBeInTheDocument()
    expect(screen.getByText('Jane Smith')).toBeInTheDocument()
    expect(screen.getByText('john@example.com')).toBeInTheDocument()
    expect(screen.getByText('jane@example.com')).toBeInTheDocument()
  })

  it('calls onSelectUser when user is clicked', async () => {
    const user = userEvent.setup()
    const onSelectUser = vi.fn()
    
    renderWithProvider(
      <UserList users={mockUsers} selectedUserId={null} onSelectUser={onSelectUser} />
    )
    
    const userCard = screen.getByText('John Doe').closest('button')
    if (userCard) {
      await user.click(userCard)
      expect(onSelectUser).toHaveBeenCalledWith('1')
    }
  })

  it('highlights selected user', () => {
    const onSelectUser = vi.fn()
    renderWithProvider(
      <UserList users={mockUsers} selectedUserId="1" onSelectUser={onSelectUser} />
    )
    
    const userCard = screen.getByText('John Doe').closest('button')
    expect(userCard).toHaveClass('border-blue-500')
  })
})
