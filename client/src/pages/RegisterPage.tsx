
import { Form, useActionData, useNavigation } from 'react-router'
import type { registerAction } from '../auth/registerAction'

export default function RegisterPage() {
  const result = useActionData<typeof registerAction>()
  const submitting = useNavigation().state === 'submitting'
  const fieldErrors = result?.fieldErrors ?? {}
  const hasFieldErrors = Object.keys(fieldErrors).length > 0
  function errorFor(field: string) {
    const messages = fieldErrors[field]
    return messages && <small role="alert">{messages.join(' ')}</small>
  }

  return (
    
      
      <Form method="post"  className="form">
        <h1>Register</h1>
        <label>
          Username
          <input name="userName" autoComplete="username" required />
        </label>
        {errorFor('UserName')}

        <label>
          Password
          <input name="password" type="password" autoComplete="new-password" required />
        </label>
        {errorFor('Password')}

        <label>
          Known as
          <input name="knownAs" required />
        </label>
        {errorFor('KnownAs')}

        <label>
          Gender
          <select name="gender" required defaultValue="">
            <option value="" disabled>Choose…</option>
            <option value="male">Male</option>
            <option value="female">Female</option>
          </select>
        </label>
        {errorFor('Gender')}

        <label>
          Date of birth
          <input name="dateOfBirth" type="date" required />
        </label>
        {errorFor('DateOfBirth')}

        <label>
          City
          <input name="city" required />
        </label>
        {errorFor('City')}

        <label>
          Country
          <input name="country" required />
        </label>
        {errorFor('Country')}

      {result?.error && !hasFieldErrors && <p role="alert">{result.error}</p>}
        <button type="submit" disabled={submitting}>
          {submitting ? 'Creating account…' : 'Register'}
        </button>
      </Form>
    
  )
}