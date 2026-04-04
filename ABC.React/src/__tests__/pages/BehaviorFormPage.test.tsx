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
import { BehaviorFormPage } from "../../pages/BehaviorFormPage"
import { GET_BEHAVIORS } from "../../graphql/operations/behaviorOperations"

function renderWithProviders(mocks: MockDef[], route = "/behavior/manage") {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter initialEntries={[route]}>
        <BehaviorFormPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("BehaviorFormPage", () => {
  it("renders the Add form for a new behavior", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: { data: { behaviors: { nodes: [] } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Add Behavior")).toBeInTheDocument()
    })
  })

  it("renders the Edit form when id is in search params", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: {
            behaviors: {
              nodes: [
                { id: "1", name: "Hand flapping", description: "Repetitive hand movements" },
              ],
            },
          },
        },
      },
    ]
    renderWithProviders(mocks, "/behavior/manage?id=1")

    await waitFor(() => {
      expect(screen.getByText("Edit Behavior")).toBeInTheDocument()
    })

    expect(screen.getByLabelText("Behavior Name")).toHaveValue("Hand flapping")
  })
})
