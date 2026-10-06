const TOKEN_KEY = 'token'

export function getToken(): string | null {
  const token = localStorage.getItem(TOKEN_KEY)
  if (!token) return null
  if (isExpired(token)) {
    localStorage.removeItem(TOKEN_KEY)
    return null
  }
  return token
}

export function setToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token)
}

export function clearToken() {
  localStorage.removeItem(TOKEN_KEY)
}

// A JWT is header.payload.signature; the payload is base64url JSON with `exp` in seconds.
function isExpired(token: string): boolean {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    const { exp } = JSON.parse(atob(payload))
    return typeof exp !== 'number' || exp * 1000 <= Date.now()
  } catch {
    return true
  }
}