import { redirect, type ActionFunctionArgs } from 'react-router'
import { ApiError } from '../auth/api/client'
import { getUserId } from '../auth/tokenStorage'
import { updateProfile, type UpdateProfileRequest } from './api/members'

export async function editProfileAction({ request }: ActionFunctionArgs) {
  const form = await request.formData()
  const data: UpdateProfileRequest = {
    city: String(form.get('city')),
    country: String(form.get('country')),
    introduction: String(form.get('introduction')),
    lookingFor: String(form.get('lookingFor')),
    interests: String(form.get('interests')),
  }

  try {
    await updateProfile(data)
    return redirect(`/members/${getUserId()}`)
  } catch (error) {
    if (error instanceof ApiError) {
      return { error: error.message, fieldErrors: error.problem.errors ?? {} }
    }
    throw error
  }
}