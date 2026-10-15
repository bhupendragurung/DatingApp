
import { redirect, type ActionFunctionArgs } from 'react-router'
import { login, register, type RegisterRequest } from './api/account'
import { ApiError } from './api/client'
import { setToken } from './tokenStorage'

export async function registerAction({ request }: ActionFunctionArgs) {
  const form = await request.formData()
  const data: RegisterRequest = {
    userName: String(form.get('userName')),
    password: String(form.get('password')),
    knownAs: String(form.get('knownAs')),
    gender: String(form.get('gender')) as RegisterRequest['gender'],
    dateOfBirth: String(form.get('dateOfBirth')),
    city: String(form.get('city')),
    country: String(form.get('country')),
  }

  try {
    await register(data)
    const user = await login({ userName: data.userName, password: data.password })
    setToken(user.token)
    return redirect('/')
  } catch (error) {
    if (error instanceof ApiError) {
      return { error: error.message, fieldErrors: error.problem.errors ?? {} }
    }
    throw error
  }
}