import { Form, useActionData, useNavigation } from 'react-router'
import type { loginAction } from '../auth/loginAction'




export default function LoginPage() {
  const result = useActionData<typeof loginAction>()
  const submitting = useNavigation().state === 'submitting'

  return (
   
      <Form method="post"  className="form">
        <h1>Log in</h1>
        <label>
          Username
          <input name="userName" autoComplete="username" required />
        </label>
        <label>
          Password
          <input name="password" type="password" autoComplete="current-password" required />
        </label>
        {result?.error && <p role="alert">{result.error}</p>}
        <button type="submit" disabled={submitting}>
          {submitting ? 'Logging in…' : 'Log in'}
        </button>
      </Form>
    
  )
}