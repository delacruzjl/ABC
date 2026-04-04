import React from "react"
import { renderHook, waitFor } from "@testing-library/react"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"
import { useAntecedents } from "../../hooks/useAntecedents"
import {
  GET_ANTECEDENTS,
  CREATE_ANTECEDENT,
  REMOVE_ANTECEDENT,
} from "../../graphql/operations/antecedentOperations"

const mockAntecedents = [
  { id: "1", name: "Loud noise", description: "A sudden loud noise" },
  { id: "2", name: "Crowded room", description: "Being in a crowded space" },
]

interface MockDef {
  request: { query: any; variables?: any }
  result?: { data: any }
  error?: Error
}

function createWrapper(mocks: MockDef[]) {
  const link = new MockLink(mocks, { addTypename: false })
  const client = new ApolloClient({
    link,
    cache: new InMemoryCache({ addTypenames: false }),
  })
  return function Wrapper({ children }: { children: React.ReactNode }) {
    return <ApolloProvider client={client}>{children}</ApolloProvider>
  }
}

describe("useAntecedents", () => {
  it("returns antecedents from the query", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: { antecedents: { nodes: mockAntecedents } },
        },
      },
    ]

    const { result } = renderHook(() => useAntecedents(), {
      wrapper: createWrapper(mocks),
    })

    expect(result.current.loading).toBe(true)
    expect(result.current.antecedents).toEqual([])

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.antecedents).toEqual(mockAntecedents)
    expect(result.current.error).toBeUndefined()
  })

  it("returns error when query fails", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        error: new Error("Network error"),
      },
    ]

    const { result } = renderHook(() => useAntecedents(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.error).toBeDefined()
    expect(result.current.antecedents).toEqual([])
  })

  it("createAntecedent calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: { antecedents: { nodes: [] } },
        },
      },
      {
        request: {
          query: CREATE_ANTECEDENT,
          variables: { name: "Test", description: "Test desc" },
        },
        result: {
          data: {
            createAntecedent: {
              id: "3",
              name: "Test",
              description: "Test desc",
            },
          },
        },
      },
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: {
            antecedents: {
              nodes: [{ id: "3", name: "Test", description: "Test desc" }],
            },
          },
        },
      },
    ]

    const { result } = renderHook(() => useAntecedents(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.createAntecedent("Test", "Test desc")

    await waitFor(() => {
      expect(result.current.antecedents).toEqual([
        { id: "3", name: "Test", description: "Test desc" },
      ])
    })
  })

  it("removeAntecedent calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: { antecedents: { nodes: mockAntecedents } },
        },
      },
      {
        request: {
          query: REMOVE_ANTECEDENT,
          variables: { antecedentId: "1" },
        },
        result: {
          data: { removeAntecedent: true },
        },
      },
      {
        request: { query: GET_ANTECEDENTS },
        result: {
          data: {
            antecedents: { nodes: [mockAntecedents[1]] },
          },
        },
      },
    ]

    const { result } = renderHook(() => useAntecedents(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.removeAntecedent("1")

    await waitFor(() => {
      expect(result.current.antecedents).toEqual([mockAntecedents[1]])
    })
  })

  it("returns empty array when data is undefined", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: { data: { antecedents: { nodes: [] } } },
      },
    ]

    const { result } = renderHook(() => useAntecedents(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.antecedents).toEqual([])
  })
})
