import { apiFetch } from '../../auth/api/client'

export type Member = {
  id: string
  userName: string
  knownAs: string
  age: number
  gender: string
  city: string
  country: string
  lastActive: string
}
export type MemberDetail = Member & {
  createdAt: string
  introduction: string | null
  lookingFor: string | null
  interests: string | null
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
  orderBy?: 'newest' | 'name' | 'lastActive'
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
export function getMember(id: string): Promise<MemberDetail> {
  return apiFetch<MemberDetail>(`/members/${encodeURIComponent(id)}`)
}
export type UpdateProfileRequest = {
  city: string
  country: string
  introduction: string | null
  lookingFor: string | null
  interests: string | null
}
export function updateProfile(data: UpdateProfileRequest): Promise<void> {
  return apiFetch<void>('/members', {
    method: 'PUT',
    body: JSON.stringify(data),
  })
}