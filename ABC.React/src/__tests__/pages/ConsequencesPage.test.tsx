import React from "react"
import { render, screen, waitFor } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"

interface MockDef {
  request: { query: any; variables?: any }
  result?: { data: any }
  error?: Error
}
import { MemoryRouter } from "react-router-dom"
import { ConsequencesPage } from "../../pages/ConsequencesPage"
import { GET_CONSEQUENCES } from "../../graphql/operations/consequenceOperations"

const mockConsequences = [
  { id: "1", name: "Timeout", description: "Brief removal from activity" },
]

function renderWithProviders(mocks: MockDef[]) {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter>
        <ConsequencesPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("ConsequencesPage", () => {
  it("shows loading state initially", () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: { data: { consequences: { nodes: mockConsequences } } },
      },
    ]
    renderWithProviders(mocks)
    expect(screen.getByText(/Loading consequences/)).toBeInTheDocument()
  })

  it("renders consequences after loading", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: { data: { consequences: { nodes: mockConsequences } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Timeout")).toBeInTheDocument()
    })
  })

  it("shows error state on failure", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        error: new Error("Failed to fetch"),
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText(/Error:/)).toBeInTheDocument()
    })
  })
})
