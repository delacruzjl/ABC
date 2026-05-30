import { useQuery } from "@apollo/client/react"
import { DASHBOARD_QUERY } from "../graphql/operations/dashboardOperations"
import type {
  Observation,
  ItemWithObservationCount,
} from "../types/observation"

interface ObservationWithChild {
  id: string
  child?: { id: string; firstName: string; lastName: string } | null
}

interface EntityWithObservations {
  id: string
  name: string
  observations: ObservationWithChild[]
}

interface DashboardData {
  antecedents: { nodes: EntityWithObservations[] }
  behaviors: { nodes: EntityWithObservations[] }
  consequences: { nodes: EntityWithObservations[] }
  observations: { nodes: Observation[] }
}

export interface ChildBreakdownItem {
  childName: string
  observationCount: number
}

export interface ItemWithChildData extends ItemWithObservationCount {
  childCount: number
  childBreakdown: ChildBreakdownItem[]
}

function toTopItemsWithChildren(
  entities: EntityWithObservations[],
  limit: number
): ItemWithChildData[] {
  return [...entities]
    .map((e) => {
      const observations = e.observations ?? []
      const childMap = new Map<string, { name: string; count: number }>()

      for (const obs of observations) {
        if (obs.child) {
          const key = obs.child.id
          const existing = childMap.get(key)
          if (existing) {
            existing.count++
          } else {
            childMap.set(key, {
              name: `${obs.child.firstName} ${obs.child.lastName}`,
              count: 1,
            })
          }
        }
      }

      const childBreakdown = Array.from(childMap.values())
        .map((c) => ({ childName: c.name, observationCount: c.count }))
        .sort((a, b) => b.observationCount - a.observationCount)

      return {
        id: e.id,
        name: e.name,
        observationCount: observations.length,
        childCount: childMap.size,
        childBreakdown,
      }
    })
    .sort((a, b) => b.observationCount - a.observationCount)
    .slice(0, limit)
}

function toRecentObservations(
  observations: Observation[],
  limit: number
): Observation[] {
  return [...observations]
    .filter((o) => o.when?.startedAt)
    .sort(
      (a, b) =>
        new Date(b.when.startedAt!).getTime() -
        new Date(a.when.startedAt!).getTime()
    )
    .slice(0, limit)
}

export function useDashboard() {
  const { data, loading, error } = useQuery<DashboardData>(DASHBOARD_QUERY, {
    fetchPolicy: "cache-and-network",
  })

  const topAntecedents = toTopItemsWithChildren(
    data?.antecedents?.nodes ?? [],
    10
  )
  const topBehaviors = toTopItemsWithChildren(
    data?.behaviors?.nodes ?? [],
    10
  )
  const topConsequences = toTopItemsWithChildren(
    data?.consequences?.nodes ?? [],
    10
  )
  const recentObservations = toRecentObservations(
    data?.observations?.nodes ?? [],
    3
  )

  return {
    topAntecedents,
    topBehaviors,
    topConsequences,
    recentObservations,
    loading,
    error,
  }
}
