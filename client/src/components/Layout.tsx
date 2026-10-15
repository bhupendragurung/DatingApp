import { Link, NavLink, Outlet } from 'react-router'

export default function Layout() {
  return (
    <>
      <header className="navbar">
        <Link to="/" className="brand">Dating App</Link>
        <NavLink to="/" end>Home</NavLink>
        <NavLink to="/login">Log in</NavLink>
        <NavLink to="/register">Register</NavLink>
      </header>
      <main>
        <Outlet />
      </main>
    </>
  )
}