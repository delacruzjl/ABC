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
import { AntecedentFormPage } from "../../pages/AntecedentFormPage"
import { GET_ANTECEDENTS } from "../../graphql/operations/antecedentOperations"

function renderWithProviders(mocks: MockDef[], route = "/antecedent/manage") {
  return render(
    <ApolloProvider client={new ApolloClient({ link: new MockLink(mocks, { addTypename: false }), cache: new InMemoryCache({ addTypenames: false }) })}>
      <MemoryRouter initialEntries={[route]}>
        <AntecedentFormPage />
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("AntecedentFormPage", () => {
  it("renders the Add form for a new antecedent", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: { data: { antecedents: { nodes: [] } } },
      },
    ]
    renderWithProviders(mocks)

    await waitFor(() => {
      expect(screen.getByText("Add Antecedent")).toBeInTheDocument()
    })
  })

  it("renders the Edit form when id is in search params", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: {
            antecedents: {
              nodes: [
                { id: "1", name: "Loud noise", description: "A sudden loud noise" },
              ],
            },
          },
        },
      },
    ]
    renderWithProviders(mocks, "/antecedent/manage?id=1")

    await waitFor(() => {
      expect(screen.getByText("Edit Antecedent")).toBeInTheDocument()
    })

    expect(screen.getByLabelText("Antecedent Name")).toHaveValue("Loud noise")
  })
})
