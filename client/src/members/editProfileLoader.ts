import { redirect } from 'react-router'
import { getUserId } from '../auth/tokenStorage'
import { getMember } from './api/members'

export function editProfileLoader() {
  const id = getUserId()
  if (!id) throw redirect('/login')
  return getMember(id)
}