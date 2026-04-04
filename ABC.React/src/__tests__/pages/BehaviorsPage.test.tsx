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
import { BehaviorsPage } from "../../pages/BehaviorsPage"
import { GET_BEHAVIORS } from "../../graphql/operations/behaviorOperations"

const mockBehaviors = [
  { id: "1", name: "Hand flapping", description: "Repetitive hand movements" },
]

function renderWithProviders(mocks: MockDef[]) {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter>
        <BehaviorsPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("BehaviorsPage", () => {
  it("shows loading state initially", () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: { data: { behaviors: { nodes: mockBehaviors } } },
      },
    ]
    renderWithProviders(mocks)
    expect(screen.getByText(/Loading behaviors/)).toBeInTheDocument()
  })

  it("renders behaviors after loading", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: { data: { behaviors: { nodes: mockBehaviors } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Hand flapping")).toBeInTheDocument()
    })
  })

  it("shows error state on failure", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        error: new Error("Failed to fetch"),
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText(/Error:/)).toBeInTheDocument()
    })
  })
})
