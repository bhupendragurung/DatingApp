import { render, screen } from '@testing-library/react'
import { createMemoryRouter, RouterProvider } from 'react-router'
import { describe, expect, it } from 'vitest'
import { routes } from './router'

function renderAt(url: string) {
  const router = createMemoryRouter(routes, { initialEntries: [url] })
  render(<RouterProvider router={router} />)
}

describe('routes', () => {
  it('shows the home page at /', async () => {
    renderAt('/')
    expect(await screen.findByRole('heading', { name: 'Find your match' })).toBeInTheDocument()
  })

  it('shows the not-found page for an unknown URL', async () => {
    renderAt('/no-such-page')
    expect(await screen.findByRole('heading', { name: 'Page not found' })).toBeInTheDocument()
  })
})