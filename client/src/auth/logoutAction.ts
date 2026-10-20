import { redirect } from 'react-router'
import { clearToken } from './tokenStorage'

export function logoutAction() {
  clearToken()
  return redirect('/')
}