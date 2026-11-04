import { createBrowserRouter, type RouteObject } from 'react-router'
import Layout from './components/Layout'
import HomePage from './pages/HomePage'
import NotFoundPage from './pages/NotFoundPage'
import LoginPage from './pages/LoginPage'
import { loginAction } from './auth/loginAction'
import RegisterPage from './pages/RegisterPage'
import { registerAction } from './auth/registerAction'
import MembersPage from './pages/MembersPage'
import { requireAuth } from './auth/guards'
import { rootLoader } from './auth/rootLoader'
import { logoutAction } from './auth/logoutAction'
import ErrorPage from './pages/ErrorPage'
import { membersLoader } from './members/membersLoader'
export const routes: RouteObject[] = [
  {
    id: 'root',
    path: '/',
    element: <Layout />,
    loader: rootLoader,
    hydrateFallbackElement: <p>Loading…</p>,
    children: [
      {
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <HomePage /> },
          { path: '*', element: <NotFoundPage /> },
          { path: 'login', element: <LoginPage />, action: loginAction },
          { path: 'register', element: <RegisterPage />, action: registerAction },
          { path: 'logout', action: logoutAction },
          {
            loader: requireAuth,
            children: [{ path: 'members', element: <MembersPage />, loader: membersLoader }],
          },
        ],
      },
    ],
  },
]

export const router = createBrowserRouter(routes)