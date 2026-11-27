import { apiFetch } from '../../auth/api/client'
import type { Member, PagedResult } from '../../members/api/members'

export type LikesPredicate = 'liked' | 'likedBy'

export function likeUser(targetUserId: string): Promise<void> {
  return apiFetch<void>(`/likes/${encodeURIComponent(targetUserId)}`, { method: 'POST' })
}

export function getLikedIds(): Promise<string[]> {
  return apiFetch<string[]>('/likes/ids')
}

export function getLikes(predicate: LikesPredicate, page = 1): Promise<PagedResult<Member>> {
  const params = new URLSearchParams({ predicate, page: String(page) })
  return apiFetch<PagedResult<Member>>(`/likes?${params}`)
}