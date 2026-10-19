
import { redirect, type LoaderFunctionArgs } from 'react-router'
import { getToken } from './tokenStorage'

// Runs before any protected route renders. No valid token -> send to login,
// remembering where the user was trying to go.
export function requireAuth({ request }: LoaderFunctionArgs) {
  if (getToken()) return null

  const url = new URL(request.url)
  const redirectTo = url.pathname + url.search
  return redirect(`/login?redirectTo=${encodeURIComponent(redirectTo)}`)
}
