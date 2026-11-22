import { useEffect, useRef } from 'react'
import { useFetcher } from 'react-router'
import type { uploadPhotoAction } from './uploadPhotoAction'

export default function PhotoUploadForm() {
  const fetcher = useFetcher<typeof uploadPhotoAction>()
  const formRef = useRef<HTMLFormElement>(null)
  const uploading = fetcher.state !== 'idle'

  useEffect(() => {
    if (fetcher.state === 'idle' && fetcher.data && 'ok' in fetcher.data) {
      formRef.current?.reset()
    }
  }, [fetcher.state, fetcher.data])

  return (
    <fetcher.Form
      ref={formRef}
      method="post"
      action="/profile/photos"
      encType="multipart/form-data"
      className="form"
    >
      <label>
        Add a photo
        <input type="file" name="file" accept="image/jpeg,image/png,image/webp" required />
      </label>

      {fetcher.data && 'error' in fetcher.data && <p role="alert">{fetcher.data.error}</p>}

      <button type="submit" disabled={uploading}>
        {uploading ? 'Uploading…' : 'Upload'}
      </button>
    </fetcher.Form>
  )
}