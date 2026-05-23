import React from "react"
import { render, screen, waitFor, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"
import { MemoryRouter } from "react-router-dom"
import { DashboardPage } from "../../pages/DashboardPage"
import { AuthProvider } from "../../context/AuthContext"
import {
  DASHBOARD_QUERY,
} from "../../graphql/operations/dashboardOperations"

// Mock recharts
jest.mock("recharts", () => {
  const OriginalModule = jest.requireActual("recharts")
  return {
    ...OriginalModule,
    ResponsiveContainer: ({ children }: { children: React.ReactNode }) => (
      <div data-testid="responsive-container">{children}</div>
    ),
  }
})

interface MockDef {
  request: { query: any; variables?: any }
  result?: { data: any }
  error?: Error
}

const emptyMocks: MockDef[] = [
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

function renderWithProviders(mocks: MockDef[]) {
  const link = new MockLink(mocks, { addTypename: false })
  const client = new ApolloClient({
    link,
    cache: new InMemoryCache({ addTypenames: false }),
  })
  return render(
    <ApolloProvider client={client}>
      <AuthProvider>
        <MemoryRouter>
          <DashboardPage />
        </MemoryRouter>
      </AuthProvider>
    </ApolloProvider>
  )
}

describe("DashboardPage", () => {
  it("shows loading state initially", () => {
    renderWithProviders(emptyMocks)
    expect(screen.getByText(/Loading dashboard/)).toBeInTheDocument()
  })

  it("renders the dashboard heading", async () => {
    renderWithProviders(emptyMocks)

    await waitFor(() => {
      expect(screen.getByText("Dashboard")).toBeInTheDocument()
    })
  })

  it("renders children navigation card", () => {
    renderWithProviders(emptyMocks)
    expect(screen.getByText("Children")).toBeInTheDocument()
  })

  it("renders chart sections after loading", async () => {
    renderWithProviders(emptyMocks)

    await waitFor(() => {
      expect(
        screen.getByText("Top 10 by Observation Count")
      ).toBeInTheDocument()
    })

    expect(screen.getByText("Top Antecedents")).toBeInTheDocument()
    expect(screen.getByText("Top Behaviors")).toBeInTheDocument()
    expect(screen.getByText("Top Consequences")).toBeInTheDocument()
  })

  it("renders recent observations section after loading", async () => {
    renderWithProviders(emptyMocks)

    await waitFor(() => {
      expect(
        screen.getByText("3 Most Recent Observations")
      ).toBeInTheDocument()
    })
  })

  it("shows error message on query failure", async () => {
    const errorMocks: MockDef[] = [
      {
        request: { query: DASHBOARD_QUERY },
        error: new Error("Network failure"),
      },
    ]

    renderWithProviders(errorMocks)

    await waitFor(() => {
      expect(screen.getByText(/Error loading data/)).toBeInTheDocument()
    })
  })
})
