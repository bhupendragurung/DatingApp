import {Form, Link, NavLink, Outlet, useLoaderData, useNavigation } from 'react-router'
import type { rootLoader } from '../auth/rootLoader'
export default function Layout() {
  const { userName } = useLoaderData<typeof rootLoader>()
  const busy = useNavigation().state !== 'idle'
  return (
    <>
      {busy && <div className="loading-bar" role="progressbar" aria-label="Loading" />}
      <header className="navbar">
        <Link to="/" className="brand">Dating App</Link>
        <NavLink to="/" end>Home</NavLink>
        {userName ? (
          <>
            <NavLink to="/members">Members</NavLink>
            <NavLink to="/profile/edit">Edit profile</NavLink>
            <span>Hi, {userName}</span>
             <Form method="post" action="/logout">
              <button type="submit">Log out</button>
            </Form>
          </>
        ) : (
          <>
            <NavLink to="/login">Log in</NavLink>
            <NavLink to="/register">Register</NavLink>
          </>
        )}
      </header>
      <main>
        <Outlet />
      </main>
    </>
  )
}