import type { ActionFunctionArgs } from 'react-router'
import { ApiError } from '../auth/api/client'
import { deletePhoto, setMainPhoto } from './api/members'

export async function photoAction({ request, params }: ActionFunctionArgs) {
  const photoId = params.photoId
  if (!photoId) throw new Response('Not found', { status: 404 })

  try {
    if (request.method === 'PUT') {
      await setMainPhoto(photoId)
    } else if (request.method === 'DELETE') {
      await deletePhoto(photoId)
    } else {
      throw new Response('Method not allowed', { status: 405 })
    }
    return { ok: true }
  } catch (error) {
    if (error instanceof ApiError) {
      return { error: error.message }
    }
    throw error
  }
}