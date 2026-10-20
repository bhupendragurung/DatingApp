import { describe, expect, it } from 'vitest'
import { safeRedirect } from './loginAction'

describe('safeRedirect', () => {
  it.each([
    ['/members', '/members'],
    ['/members?page=2', '/members?page=2'],
    [null, '/'],
    ['', '/'],
    ['https://evil.com', '/'],
    ['//evil.com', '/'],
    ['/\\evil.com', '/'],
  ])('turns %s into %s', (input, expected) => {
    expect(safeRedirect(input)).toBe(expected)
  })
})