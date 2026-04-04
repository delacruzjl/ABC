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
import { ConsequenceFormPage } from "../../pages/ConsequenceFormPage"
import { GET_CONSEQUENCES } from "../../graphql/operations/consequenceOperations"

function renderWithProviders(mocks: MockDef[], route = "/consequence/manage") {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter initialEntries={[route]}>
        <ConsequenceFormPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("ConsequenceFormPage", () => {
  it("renders the Add form for a new consequence", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: { data: { consequences: { nodes: [] } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Add Consequence")).toBeInTheDocument()
    })
  })

  it("renders the Edit form when id is in search params", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: {
            consequences: {
              nodes: [
                { id: "1", name: "Timeout", description: "Brief removal from activity" },
              ],
            },
          },
        },
      },
    ]
    renderWithProviders(mocks, "/consequence/manage?id=1")

    await waitFor(() => {
      expect(screen.getByText("Edit Consequence")).toBeInTheDocument()
    })

    expect(screen.getByLabelText("Consequence Name")).toHaveValue("Timeout")
  })
})
