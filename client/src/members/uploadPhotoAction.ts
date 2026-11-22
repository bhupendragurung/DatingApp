import type { ActionFunctionArgs } from 'react-router'
import { ApiError } from '../auth/api/client'
import { uploadPhoto } from './api/members'

export async function uploadPhotoAction({ request }: ActionFunctionArgs) {
  const form = await request.formData()
  const file = form.get('file')

  if (!(file instanceof File) || file.size === 0) {
    return { error: 'Choose a photo to upload.' }
  }

  try {
    await uploadPhoto(file)
    return { ok: true }
  } catch (error) {
    if (error instanceof ApiError) {
      const messages = Object.values(error.problem.errors ?? {}).flat()
      return { error: messages.length > 0 ? messages.join(' ') : error.message }
    }
    throw error
  }
}