import { render, screen } from '@testing-library/react'
import { createMemoryRouter, RouterProvider } from 'react-router'
import { beforeEach, describe, expect, it } from 'vitest'
import { routes } from './router'
import { setToken } from './auth/tokenStorage'

function renderAt(url: string) {
  const router = createMemoryRouter(routes, { initialEntries: [url] })
  render(<RouterProvider router={router} />)
  return router
}

// The client only reads the payload; the API is what checks the signature,
// so header and signature can be dummy text here.
function fakeToken(userName: string, secondsFromNow = 3600) {
  const exp = Math.floor(Date.now() / 1000) + secondsFromNow
  return `header.${btoa(JSON.stringify({ unique_name: userName, exp }))}.signature`
}

beforeEach(() => localStorage.clear())

describe('routes', () => {
  it('shows the home page at /', async () => {
    renderAt('/')
    expect(await screen.findByRole('heading', { name: 'Find your match' })).toBeInTheDocument()
  })

  it('shows the not-found page for an unknown URL', async () => {
    renderAt('/no-such-page')
    expect(await screen.findByRole('heading', { name: 'Page not found' })).toBeInTheDocument()
  })

  it('sends a logged-out user from a protected page to login', async () => {
    const router = renderAt('/members')
    expect(await screen.findByRole('heading', { name: 'Log in' })).toBeInTheDocument()
    expect(router.state.location.pathname).toBe('/login')
    expect(router.state.location.search).toBe('?redirectTo=%2Fmembers')
  })

  it('shows a protected page and the username to a logged-in user', async () => {
    setToken(fakeToken('alice'))
    renderAt('/members')
    expect(await screen.findByRole('heading', { name: 'Members' })).toBeInTheDocument()
    expect(screen.getByText('Hi, alice')).toBeInTheDocument()
  })

  it('treats an expired token as logged out', async () => {
    setToken(fakeToken('alice', -60))
    renderAt('/members')
    expect(await screen.findByRole('heading', { name: 'Log in' })).toBeInTheDocument()
  })
})