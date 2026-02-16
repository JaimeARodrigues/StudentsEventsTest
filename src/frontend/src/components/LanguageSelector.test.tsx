import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import LanguageSelector from '../components/LanguageSelector'
import { LanguageProvider } from '../i18n/LanguageContext'

const renderWithProvider = (component: React.ReactElement) => {
  return render(
    <LanguageProvider>
      {component}
    </LanguageProvider>
  )
}

describe('LanguageSelector', () => {
  it('renders language selector button', () => {
    renderWithProvider(<LanguageSelector />)
    
    const button = screen.getByRole('button', { name: /select language/i })
    expect(button).toBeInTheDocument()
  })

  it('displays current language flag', () => {
    renderWithProvider(<LanguageSelector />)
    
    const button = screen.getByRole('button', { name: /select language/i })
    expect(button.textContent).toContain('🇧🇷')
  })

  it('opens dropdown when clicked', async () => {
    const user = userEvent.setup()
    renderWithProvider(<LanguageSelector />)
    
    const button = screen.getByRole('button', { name: /select language/i })
    await user.click(button)
    
    expect(screen.getByText('Português (BR)')).toBeInTheDocument()
    expect(screen.getByText('English')).toBeInTheDocument()
  })

  it('changes language when option is selected', async () => {
    const user = userEvent.setup()
    renderWithProvider(<LanguageSelector />)
    
    const button = screen.getByRole('button', { name: /select language/i })
    await user.click(button)
    
    const englishOption = screen.getByText('English')
    await user.click(englishOption)
    
    expect(button.textContent).toContain('🇺🇸')
  })

  it('closes dropdown after selecting language', async () => {
    const user = userEvent.setup()
    renderWithProvider(<LanguageSelector />)
    
    const button = screen.getByRole('button', { name: /select language/i })
    await user.click(button)
    
    const englishOption = screen.getByText('English')
    await user.click(englishOption)
    
    expect(screen.queryByText('Português (BR)')).not.toBeInTheDocument()
  })
})
