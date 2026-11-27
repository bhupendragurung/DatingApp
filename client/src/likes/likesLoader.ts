import type { LoaderFunctionArgs } from 'react-router'
import { getLikedIds, getLikes, type LikesPredicate } from './api/likes'

export async function likesLoader({ request }: LoaderFunctionArgs) {
  const params = new URL(request.url).searchParams
  const predicate: LikesPredicate = params.get('predicate') === 'likedBy' ? 'likedBy' : 'liked'
  const page = Number(params.get('page')) || 1

  const [likes, likedIds] = await Promise.all([getLikes(predicate, page), getLikedIds()])
  return { ...likes, predicate, likedIds }
}