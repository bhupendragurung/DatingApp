
import { useFetcher } from 'react-router'
import type { Photo } from './api/members'
import type { photoAction } from './photoAction'

export default function PhotoManager({ photos }: { photos: Photo[] }) {
  if (photos.length === 0) return <p>No photos yet.</p>

  return (
    <ul className="photo-gallery photo-manager">
      {photos.map((photo) => (
        <PhotoItem key={photo.id} photo={photo} />
      ))}
    </ul>
  )
}
function PhotoItem({ photo }: { photo: Photo }) {
  const fetcher = useFetcher<typeof photoAction>()
  const busy = fetcher.state !== 'idle'
  const action = `/profile/photos/${photo.id}`

  function confirmDelete(event: React.FormEvent<HTMLFormElement>) {
    if (!window.confirm('Delete this photo?')) event.preventDefault()
  }

  return (
    <li>
      <img src={photo.url} alt={photo.isMain ? 'Your main photo' : 'Your photo'} />

      {photo.isMain ? (
        <span className="photo-badge">Main photo</span>
      ) : (
        <div className="photo-actions">
          <fetcher.Form method="put" action={action}>
            <button type="submit" disabled={busy}>Set main</button>
          </fetcher.Form>
          <fetcher.Form method="delete" action={action} onSubmit={confirmDelete}>
            <button type="submit" disabled={busy}>Delete</button>
          </fetcher.Form>
        </div>
      )}

      {fetcher.data && 'error' in fetcher.data && (
        <small role="alert">{fetcher.data.error}</small>
      )}
    </li>
  )
}