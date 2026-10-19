import { redirect, type ActionFunctionArgs } from 'react-router'
import { login } from './api/account'
import { ApiError } from './api/client'
import { setToken } from './tokenStorage'

export async function loginAction({ request }: ActionFunctionArgs) {
  const redirectTo = safeRedirect(new URL(request.url).searchParams.get('redirectTo'))
  const form = await request.formData()
  try {
    const user = await login({
      userName: String(form.get('userName')),
      password: String(form.get('password')),
    })
    setToken(user.token)
    return redirect(redirectTo)
  } catch (error) {
    if (error instanceof ApiError) return { error: error.message }
    throw error
  }
}

// Only allow paths inside this app. Blocks open redirects like
// /login?redirectTo=https://evil.com or //evil.com.
function safeRedirect(to: string | null): string {
  if (!to || !to.startsWith('/') || to.startsWith('//')) return '/'
  return to
}
