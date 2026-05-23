import React, { createContext, useContext, useState, useEffect, useCallback } from "react"

import { useApolloClient } from "@apollo/client/react"

interface AuthUser {
  email: string
  roles: string[]
}

interface AuthContextType {
  user: AuthUser | null
  token: string | null
  isAdmin: boolean
  isAuthenticated: boolean
  login: (token: string, email: string, roles: string[]) => void
  logout: () => void
}

const AuthContext = createContext<AuthContextType>({
  user: null,
  token: null,
  isAdmin: false,
  isAuthenticated: false,
  login: () => {},
  logout: () => {},
})

export const useAuth = () => useContext(AuthContext)

function parseToken(token: string): AuthUser | null {
  try {
    const payload = JSON.parse(atob(token.split(".")[1]))
    const email =
      payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] ??
      payload.email ??
      ""
    const roleClaim =
      payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
    const roles = Array.isArray(roleClaim) ? roleClaim : roleClaim ? [roleClaim] : []
    return { email, roles }
  } catch {
    return null
  }
}

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const client = useApolloClient()
  const [token, setToken] = useState<string | null>(() => localStorage.getItem("abc_token"))
  const [user, setUser] = useState<AuthUser | null>(() => {
    const stored = localStorage.getItem("abc_token")
    return stored ? parseToken(stored) : null
  })

  const login = useCallback((newToken: string, email: string, roles: string[]) => {
    localStorage.setItem("abc_token", newToken)
    setToken(newToken)
    setUser({ email, roles })
  }, [])

  const logout = useCallback(() => {
    localStorage.removeItem("abc_token")
    setToken(null)
    setUser(null)
    client.clearStore()
  }, [client])

  useEffect(() => {
    if (token) {
      const parsed = parseToken(token)
      if (!parsed) {
        logout()
      }
    }
  }, [token, logout])

  const isAdmin = user?.roles.includes("Admin") ?? false
  const isAuthenticated = !!token && !!user

  return (
    <AuthContext.Provider value={{ user, token, isAdmin, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}
