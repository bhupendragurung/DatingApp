import { createBrowserRouter, type RouteObject } from 'react-router'
import Layout from './components/Layout'
import HomePage from './pages/HomePage'
import NotFoundPage from './pages/NotFoundPage'
import LoginPage from './pages/LoginPage'
import { loginAction } from './auth/loginAction'

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <Layout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: '*', element: <NotFoundPage /> },
      { path: 'login', element: <LoginPage />, action: loginAction },
    ],
  },
]

export const router = createBrowserRouter(routes)