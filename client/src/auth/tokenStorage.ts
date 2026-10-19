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
export function getUserName(): string | null {
  const token = getToken()
  if (!token) return null
  const name = readPayload(token)?.unique_name
  return typeof name === 'string' ? name : null
}

function isExpired(token: string): boolean {
  const exp = readPayload(token)?.exp
  return typeof exp !== 'number' || exp * 1000 <= Date.now()
}

// A JWT is header.payload.signature; the payload is base64url JSON.
function readPayload(token: string): Record<string, unknown> | null {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    return JSON.parse(atob(payload))
  } catch {
    return null
  }
}