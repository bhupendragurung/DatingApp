import { redirect, type ActionFunctionArgs } from 'react-router'
import { login } from './api/account'
import { ApiError } from './api/client'
import { setToken } from './tokenStorage'

export async function loginAction({ request }: ActionFunctionArgs) {
  const form = await request.formData()
  try {
    const user = await login({
      userName: String(form.get('userName')),
      password: String(form.get('password')),
    })
    setToken(user.token)
    return redirect('/')
  } catch (error) {
    if (error instanceof ApiError) return { error: error.message }
    throw error
  }
}