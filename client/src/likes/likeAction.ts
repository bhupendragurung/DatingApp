import type { ActionFunctionArgs } from 'react-router'
import { ApiError } from '../auth/api/client'
import { likeUser } from './api/likes'

export async function likeAction({ params }: ActionFunctionArgs) {
  const targetUserId = params.targetUserId
  if (!targetUserId) throw new Response('Not found', { status: 404 })

  try {
    await likeUser(targetUserId)
    return { ok: true }
  } catch (error) {
    if (error instanceof ApiError) {
      return { error: error.message }
    }
    throw error
  }
}