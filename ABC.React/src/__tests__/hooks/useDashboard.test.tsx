import React from "react"
import { renderHook, waitFor } from "@testing-library/react"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"
import { useDashboard } from "../../hooks/useDashboard"
import {
  GET_ANTECEDENTS_WITH_OBSERVATIONS,
  GET_BEHAVIORS_WITH_OBSERVATIONS,
  GET_CONSEQUENCES_WITH_OBSERVATIONS,
  GET_RECENT_OBSERVATIONS,
} from "../../graphql/operations/dashboardOperations"

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

const baseMocks: MockDef[] = [
  {
    request: { query: GET_ANTECEDENTS_WITH_OBSERVATIONS },
    result: {
      data: {
        antecedents: {
          nodes: [
            { id: "1", name: "Loud noise", observations: [{ id: "o1" }, { id: "o2" }] },
            { id: "2", name: "Crowded room", observations: [{ id: "o1" }] },
            { id: "3", name: "Quiet space", observations: [] },
          ],
        },
      },
    },
  },
  {
    request: { query: GET_BEHAVIORS_WITH_OBSERVATIONS },
    result: {
      data: {
        behaviors: {
          nodes: [
            { id: "1", name: "Hand flapping", observations: [{ id: "o1" }, { id: "o2" }, { id: "o3" }] },
          ],
        },
      },
    },
  },
  {
    request: { query: GET_CONSEQUENCES_WITH_OBSERVATIONS },
    result: {
      data: {
        consequences: {
          nodes: [
            { id: "1", name: "Timeout", observations: [{ id: "o1" }] },
          ],
        },
      },
    },
  },
  {
    request: { query: GET_RECENT_OBSERVATIONS },
    result: {
      data: {
        observations: {
          nodes: [
            {
              id: "o1",
              notes: "First observation",
              status: "CLOSED",
              when: { startedAt: "2026-01-01T10:00:00Z", endedAt: "2026-01-01T11:00:00Z" },
              antecedents: [{ id: "1", name: "Loud noise" }],
              behaviors: [{ id: "1", name: "Hand flapping" }],
              consequences: [{ id: "1", name: "Timeout" }],
            },
            {
              id: "o2",
              notes: "Second observation",
              status: "OPEN",
              when: { startedAt: "2026-03-15T14:00:00Z", endedAt: null },
              antecedents: [{ id: "1", name: "Loud noise" }],
              behaviors: [],
              consequences: [],
            },
            {
              id: "o3",
              notes: "Third observation",
              status: "CLOSED",
              when: { startedAt: "2026-02-10T09:00:00Z", endedAt: "2026-02-10T10:00:00Z" },
              antecedents: [],
              behaviors: [{ id: "1", name: "Hand flapping" }],
              consequences: [],
            },
          ],
        },
      },
    },
  },
]

describe("useDashboard", () => {
  it("returns loading state initially", () => {
    const { result } = renderHook(() => useDashboard(), {
      wrapper: createWrapper(baseMocks),
    })
    expect(result.current.loading).toBe(true)
  })

  it("returns top antecedents sorted by observation count", async () => {
    const { result } = renderHook(() => useDashboard(), {
      wrapper: createWrapper(baseMocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.topAntecedents).toEqual([
      { id: "1", name: "Loud noise", observationCount: 2, childCount: 0, childBreakdown: [] },
      { id: "2", name: "Crowded room", observationCount: 1, childCount: 0, childBreakdown: [] },
      { id: "3", name: "Quiet space", observationCount: 0, childCount: 0, childBreakdown: [] },
    ])
  })

  it("returns top behaviors sorted by observation count", async () => {
    const { result } = renderHook(() => useDashboard(), {
      wrapper: createWrapper(baseMocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.topBehaviors).toEqual([
      { id: "1", name: "Hand flapping", observationCount: 3, childCount: 0, childBreakdown: [] },
    ])
  })

  it("returns recent observations sorted by date descending", async () => {
    const { result } = renderHook(() => useDashboard(), {
      wrapper: createWrapper(baseMocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.recentObservations).toHaveLength(3)
    expect(result.current.recentObservations[0].id).toBe("o2")
    expect(result.current.recentObservations[1].id).toBe("o3")
    expect(result.current.recentObservations[2].id).toBe("o1")
  })

  it("returns error when a query fails", async () => {
    const errorMocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS_WITH_OBSERVATIONS },
        error: new Error("Network error"),
      },
      {
        request: { query: GET_BEHAVIORS_WITH_OBSERVATIONS },
        result: { data: { behaviors: { nodes: [] } } },
      },
      {
        request: { query: GET_CONSEQUENCES_WITH_OBSERVATIONS },
        result: { data: { consequences: { nodes: [] } } },
      },
      {
        request: { query: GET_RECENT_OBSERVATIONS },
        result: { data: { observations: { nodes: [] } } },
      },
    ]

    const { result } = renderHook(() => useDashboard(), {
      wrapper: createWrapper(errorMocks),
    })

    await waitFor(() => {
      expect(result.current.loading).toBe(false)
    })

    expect(result.current.error).toBeDefined()
  })
})
