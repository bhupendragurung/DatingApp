import { getToken } from '../tokenStorage'

export type ProblemDetails = {
  title?: string
  status?: number
  detail?: string
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  readonly status: number
  readonly problem: ProblemDetails

  constructor(status: number, problem: ProblemDetails) {
    super(problem.detail ?? problem.title ?? `Request failed with status ${status}`)
    this.name = 'ApiError'
    this.status = status
    this.problem = problem
  }
}

export async function apiFetch<T>(path: string, init: RequestInit = {}): Promise<T> {
  const headers = new Headers(init.headers)
  if (init.body !== undefined) headers.set('Content-Type', 'application/json')

  const token = getToken()
  if (token) headers.set('Authorization', `Bearer ${token}`)

  const response = await fetch(`/api${path}`, { ...init, headers })

  if (!response.ok) {
    // A 401 from JWT bearer has an empty body, so fall back to {}.
    const problem = (await response.json().catch(() => ({}))) as ProblemDetails
    throw new ApiError(response.status, problem)
  }

  if (response.status === 204) return undefined as T
  return (await response.json()) as T
}