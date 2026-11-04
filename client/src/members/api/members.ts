import { apiFetch } from '../../auth/api/client'

export type Member = {
  id: string
  userName: string
  knownAs: string
  age: number
  gender: string
  city: string
  country: string
}

export type PagedResult<T> = {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export type MemberFilters = {
  page?: number
  pageSize?: number
  gender?: 'male' | 'female'
  minAge?: number
  maxAge?: number
  orderBy?: 'newest' | 'name'
}

export function getMembers(filters: MemberFilters = {}): Promise<PagedResult<Member>> {
  const params = new URLSearchParams()
  for (const [key, value] of Object.entries(filters)) {
    if (value !== undefined )
         params.set(key, String(value))
  }
  const query = params.toString()
  return apiFetch<PagedResult<Member>>(query ? `/members?${query}` : '/members')
}