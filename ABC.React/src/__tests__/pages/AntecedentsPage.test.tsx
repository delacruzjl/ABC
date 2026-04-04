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
import { AntecedentsPage } from "../../pages/AntecedentsPage"
import { GET_ANTECEDENTS } from "../../graphql/operations/antecedentOperations"

const mockAntecedents = [
  { id: "1", name: "Loud noise", description: "A sudden loud noise" },
]

function renderWithProviders(mocks: MockDef[]) {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter>
        <AntecedentsPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("AntecedentsPage", () => {
  it("shows loading state initially", () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: { data: { antecedents: { nodes: mockAntecedents } } },
      },
    ]
    renderWithProviders(mocks)
    expect(screen.getByText(/Loading antecedents/)).toBeInTheDocument()
  })

  it("renders antecedents after loading", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: { data: { antecedents: { nodes: mockAntecedents } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Loud noise")).toBeInTheDocument()
    })
  })

  it("shows error state on failure", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        error: new Error("Failed to fetch"),
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText(/Error:/)).toBeInTheDocument()
    })
  })
})
