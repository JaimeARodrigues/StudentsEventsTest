import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import App from './App'

describe('App', () => {
  it('renders Event Manager heading', () => {
    render(<App />)
    expect(screen.getByText('Event Manager')).toBeInTheDocument()
  })

  it('renders loading message', () => {
    render(<App />)
    expect(screen.getByText('Loading application...')).toBeInTheDocument()
  })

  it('has correct styling classes', () => {
    const { container } = render(<App />)
    const mainDiv = container.firstChild as HTMLElement
    expect(mainDiv).toHaveClass('w-full', 'h-screen')
  })
})
