import React from "react"
import { renderHook, waitFor } from "@testing-library/react"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"

interface MockDef {
  request: { query: any; variables?: any }
  result?: { data: any }
  error?: Error
}
import { useConsequences } from "../../hooks/useConsequences"
import {
  GET_CONSEQUENCES,
  CREATE_CONSEQUENCE,
  REMOVE_CONSEQUENCE,
} from "../../graphql/operations/consequenceOperations"

const mockConsequences = [
  { id: "1", name: "Timeout", description: "Brief removal from activity" },
  { id: "2", name: "Verbal praise", description: "Positive verbal feedback" },
]

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

describe("useConsequences", () => {
  it("returns consequences from the query", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: { consequences: { nodes: mockConsequences } },
        },
      },
    ]

    const { result } = renderHook(() => useConsequences(), {
      wrapper: createWrapper(mocks),
    })

    expect(result.current.loading).toBe(true)
    expect(result.current.consequences).toEqual([])

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.consequences).toEqual(mockConsequences)
    expect(result.current.error).toBeUndefined()
  })

  it("returns error when query fails", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        error: new Error("Network error"),
      },
    ]

    const { result } = renderHook(() => useConsequences(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.error).toBeDefined()
    expect(result.current.consequences).toEqual([])
  })

  it("createConsequence calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: { consequences: { nodes: [] } },
        },
      },
      {
        request: {
          query: CREATE_CONSEQUENCE,
          variables: { name: "Test", description: "Test desc" },
        },
        result: {
          data: {
            createConsequence: {
              id: "3",
              name: "Test",
              description: "Test desc",
            },
          },
        },
      },
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: {
            consequences: {
              nodes: [{ id: "3", name: "Test", description: "Test desc" }],
            },
          },
        },
      },
    ]

    const { result } = renderHook(() => useConsequences(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.createConsequence("Test", "Test desc")

    await waitFor(() => {
      expect(result.current.consequences).toEqual([
        { id: "3", name: "Test", description: "Test desc" },
      ])
    })
  })

  it("removeConsequence calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: { consequences: { nodes: mockConsequences } },
        },
      },
      {
        request: {
          query: REMOVE_CONSEQUENCE,
          variables: { consequenceId: "1" },
        },
        result: {
          data: { removeConsequence: true },
        },
      },
      {
        request: { query: GET_CONSEQUENCES },
        result: {
          data: {
            consequences: { nodes: [mockConsequences[1]] },
          },
        },
      },
    ]

    const { result } = renderHook(() => useConsequences(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.removeConsequence("1")

    await waitFor(() => {
      expect(result.current.consequences).toEqual([mockConsequences[1]])
    })
  })

  it("returns empty array when data is undefined", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_CONSEQUENCES },
        result: { data: { consequences: { nodes: [] } } },
      },
    ]

    const { result } = renderHook(() => useConsequences(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.consequences).toEqual([])
  })
})
