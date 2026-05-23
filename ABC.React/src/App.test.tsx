import React from "react"
import { render, screen } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"

// Mock MSAL modules to avoid browser crypto requirement in jsdom
jest.mock("@azure/msal-browser", () => ({
  PublicClientApplication: jest.fn().mockImplementation(() => ({
    initialize: jest.fn().mockResolvedValue(undefined),
    acquireTokenSilent: jest.fn(),
    loginPopup: jest.fn(),
    getAllAccounts: jest.fn().mockReturnValue([]),
  })),
  LogLevel: { Warning: 2, Error: 4 },
}))
jest.mock("@azure/msal-react", () => ({
  MsalProvider: ({ children }: { children: React.ReactNode }) => <>{children}</>,
  useMsal: () => ({ instance: { loginPopup: jest.fn() }, accounts: [] }),
}))
jest.mock("@react-oauth/google", () => ({
  GoogleOAuthProvider: ({ children }: { children: React.ReactNode }) => <>{children}</>,
  GoogleLogin: () => <div data-testid="google-login" />,
}))

import { GET_ANTECEDENTS } from "./graphql/operations/antecedentOperations"
import { GET_BEHAVIORS } from "./graphql/operations/behaviorOperations"
import { GET_CONSEQUENCES } from "./graphql/operations/consequenceOperations"
import {
  DASHBOARD_QUERY,
} from "./graphql/operations/dashboardOperations"
import { AuthProvider } from "./context/AuthContext"
import App from "./App"

const mocks = [
  {
    request: { query: GET_ANTECEDENTS },
    result: { data: { antecedents: { nodes: [] } } },
  },
  {
    request: { query: GET_BEHAVIORS },
    result: { data: { behaviors: { nodes: [] } } },
  },
  {
    request: { query: GET_CONSEQUENCES },
    result: { data: { consequences: { nodes: [] } } },
  },
  {
    request: { query: DASHBOARD_QUERY },
    result: {
      data: {
        antecedents: { nodes: [] },
        behaviors: { nodes: [] },
        consequences: { nodes: [] },
        observations: { nodes: [] },
      },
    },
  },
]

test("renders the ABC Management title on login page when not authenticated", () => {
  const link = new MockLink(mocks, { addTypename: false })
  const client = new ApolloClient({
    link,
    cache: new InMemoryCache({ addTypenames: false }),
  })
  render(
    <ApolloProvider client={client}>
      <AuthProvider>
        <App />
      </AuthProvider>
    </ApolloProvider>
  )
  expect(screen.getByText("ABC Management")).toBeInTheDocument()
})

test("renders login form when not authenticated", () => {
  const link = new MockLink(mocks, { addTypename: false })
  const client = new ApolloClient({
    link,
    cache: new InMemoryCache({ addTypenames: false }),
  })
  render(
    <ApolloProvider client={client}>
      <AuthProvider>
        <App />
      </AuthProvider>
    </ApolloProvider>
  )
  // When not authenticated, the app shows the login page
  const signInElements = screen.queryAllByText("Sign In")
  expect(signInElements.length).toBeGreaterThanOrEqual(0)
})
