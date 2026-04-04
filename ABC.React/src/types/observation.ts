export interface Observation {
  id: string
  notes: string
  status: "OPEN" | "CLOSED"
  when: {
    startedAt: string | null
    endedAt: string | null
  }
  antecedents: { id: string; name: string }[]
  behaviors: { id: string; name: string }[]
  consequences: { id: string; name: string }[]
  child?: { id: string; firstName: string; lastName: string } | null
}

export interface ItemWithObservationCount {
  id: string
  name: string
  observationCount: number
}
