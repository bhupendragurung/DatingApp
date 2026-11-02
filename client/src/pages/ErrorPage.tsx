
import { isRouteErrorResponse, Link, useRouteError } from 'react-router'
import { ApiError } from '../auth/api/client'

export default function ErrorPage() {
  const error = useRouteError()
  const { title, message } = describe(error)

  return (
    <section className="error-page">
      <h1>{title}</h1>
      <p>{message}</p>
      <Link to="/">Back to home</Link>
    </section>
  )
}

function describe(error: unknown) {
  if (isRouteErrorResponse(error)) {
    return {
      title: `${error.status} ${error.statusText}`,
      message: typeof error.data === 'string' ? error.data : 'Sorry, something went wrong.',
    }
  }
  if (error instanceof ApiError) {
    return { title: `Error ${error.status}`, message: error.message }
  }
  return { title: 'Something went wrong', message: 'Please try again later.' }
}