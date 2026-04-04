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
import { useBehaviors } from "../../hooks/useBehaviors"
import {
  GET_BEHAVIORS,
  CREATE_BEHAVIOR,
  REMOVE_BEHAVIOR,
} from "../../graphql/operations/behaviorOperations"

const mockBehaviors = [
  { id: "1", name: "Hand flapping", description: "Repetitive hand movements" },
  { id: "2", name: "Screaming", description: "Loud vocal outburst" },
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

describe("useBehaviors", () => {
  it("returns behaviors from the query", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: { behaviors: { nodes: mockBehaviors } },
        },
      },
    ]

    const { result } = renderHook(() => useBehaviors(), {
      wrapper: createWrapper(mocks),
    })

    expect(result.current.loading).toBe(true)
    expect(result.current.behaviors).toEqual([])

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.behaviors).toEqual(mockBehaviors)
    expect(result.current.error).toBeUndefined()
  })

  it("returns error when query fails", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        error: new Error("Network error"),
      },
    ]

    const { result } = renderHook(() => useBehaviors(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.error).toBeDefined()
    expect(result.current.behaviors).toEqual([])
  })

  it("createBehavior calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: { behaviors: { nodes: [] } },
        },
      },
      {
        request: {
          query: CREATE_BEHAVIOR,
          variables: { name: "Test", description: "Test desc" },
        },
        result: {
          data: {
            createBehavior: {
              id: "3",
              name: "Test",
              description: "Test desc",
            },
          },
        },
      },
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: {
            behaviors: {
              nodes: [{ id: "3", name: "Test", description: "Test desc" }],
            },
          },
        },
      },
    ]

    const { result } = renderHook(() => useBehaviors(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.createBehavior("Test", "Test desc")

    await waitFor(() => {
      expect(result.current.behaviors).toEqual([
        { id: "3", name: "Test", description: "Test desc" },
      ])
    })
  })

  it("removeBehavior calls the mutation", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: { behaviors: { nodes: mockBehaviors } },
        },
      },
      {
        request: {
          query: REMOVE_BEHAVIOR,
          variables: { behaviorId: "1" },
        },
        result: {
          data: { removeBehavior: true },
        },
      },
      {
        request: { query: GET_BEHAVIORS },
        result: {
          data: {
            behaviors: { nodes: [mockBehaviors[1]] },
          },
        },
      },
    ]

    const { result } = renderHook(() => useBehaviors(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    await result.current.removeBehavior("1")

    await waitFor(() => {
      expect(result.current.behaviors).toEqual([mockBehaviors[1]])
    })
  })

  it("returns empty array when data is undefined", async () => {
    const mocks: MockDef[] = [
      {
        request: { query: GET_BEHAVIORS },
        result: { data: { behaviors: { nodes: [] } } },
      },
    ]

    const { result } = renderHook(() => useBehaviors(), {
      wrapper: createWrapper(mocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.behaviors).toEqual([])
  })
})
