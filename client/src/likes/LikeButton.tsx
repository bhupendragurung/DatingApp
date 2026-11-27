import { useFetcher } from 'react-router'
import type { likeAction } from './likeAction'

type Props = { targetUserId: string; liked: boolean }

export default function LikeButton({ targetUserId, liked }: Props) {
  const fetcher = useFetcher<typeof likeAction>()
  const pending = fetcher.state !== 'idle'
  const showLiked = liked || pending

  return (
    <fetcher.Form method="post" action={`/likes/${targetUserId}`} className="like-form">
      <button type="submit" className="like-button" disabled={showLiked} aria-pressed={showLiked}>
        {showLiked ? '♥ Liked' : '♡ Like'}
      </button>
      {fetcher.data && 'error' in fetcher.data && (
        <small role="alert">{fetcher.data.error}</small>
      )}
    </fetcher.Form>
  )
}