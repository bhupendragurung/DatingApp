import { useEffect, useState } from 'react'
import { Form, Link, useActionData, useBlocker, useLoaderData, useNavigation } from 'react-router'
import type { editProfileAction } from '../members/editProfileAction'
import type { editProfileLoader } from '../members/editProfileLoader'

export default function EditProfilePage() {
  const member = useLoaderData<typeof editProfileLoader>()
  const result = useActionData<typeof editProfileAction>()
  const submitting = useNavigation().state === 'submitting'
  const fieldErrors = result?.fieldErrors ?? {}
  const hasFieldErrors = Object.keys(fieldErrors).length > 0

  function errorFor(field: string) {
    const messages = fieldErrors[field]
    return messages && <small role="alert">{messages.join(' ')}</small>
  }
 const [isDirty, setIsDirty] = useState(false)

  const blocker = useBlocker(
    ({ currentLocation, nextLocation }) =>
      isDirty && currentLocation.pathname !== nextLocation.pathname,
  )

  useEffect(() => {
    if (!isDirty) return
    function onBeforeUnload(event: BeforeUnloadEvent) {
      event.preventDefault()
    }
    window.addEventListener('beforeunload', onBeforeUnload)
    return () => window.removeEventListener('beforeunload', onBeforeUnload)
  }, [isDirty])
  return (
    <Form method="post" className="form form-wide" onChange={() => setIsDirty(true)}>
      <h1>Edit profile</h1>

      <label>
        City
        <input name="city" defaultValue={member.city} maxLength={100} required />
      </label>
      {errorFor('City')}

      <label>
        Country
        <input name="country" defaultValue={member.country} maxLength={100} required />
      </label>
      {errorFor('Country')}

      <label>
        Introduction
        <textarea name="introduction" defaultValue={member.introduction ?? ''} maxLength={2000} rows={5} />
      </label>
      {errorFor('Introduction')}

      <label>
        Looking for
        <textarea name="lookingFor" defaultValue={member.lookingFor ?? ''} maxLength={2000} rows={5} />
      </label>
      {errorFor('LookingFor')}

      <label>
        Interests
        <textarea name="interests" defaultValue={member.interests ?? ''} maxLength={500} rows={3} />
      </label>
      {errorFor('Interests')}
 {blocker.state === 'blocked' && (
        <div className="unsaved-warning" role="alertdialog">
          <p>You have unsaved changes. Leave anyway?</p>
          <button type="button" onClick={() => blocker.proceed()}>Leave</button>
          <button type="button" onClick={() => blocker.reset()}>Stay</button>
        </div>
      )}
      {result?.error && !hasFieldErrors && <p role="alert">{result.error}</p>}

      <button type="submit" disabled={submitting}>
        {submitting ? 'Saving…' : 'Save changes'}
      </button>
      <Link to={`/members/${member.id}`}>Cancel</Link>
    </Form>
  )
}