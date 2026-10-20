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
export type RegisterRequest = {
  userName: string
  password: string
  knownAs: string
  gender: 'male' | 'female'
  dateOfBirth: string // YYYY-MM-DD
  city: string
  country: string
}
export type RegisteredUser = {
  id: string
  userName: string
  knownAs: string
}
export function register(request: RegisterRequest): Promise<RegisteredUser> {
  return apiFetch<RegisteredUser>('/account/register', {
    method: 'POST',
    body: JSON.stringify(request),
  })
}