import { apiFetch } from './client'

export type AuthUser = {
  id: string
  userName: string
  knownAs: string
  token: string
}

export type LoginRequest = {
  userName: string
  password: string
}

export function login(request: LoginRequest): Promise<AuthUser> {
  return apiFetch<AuthUser>('/account/login', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}
