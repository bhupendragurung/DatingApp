import { getUserName } from './tokenStorage'

export function rootLoader() {
  return { userName: getUserName() }
}