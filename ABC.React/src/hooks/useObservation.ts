import { useMutation, useQuery, useLazyQuery } from "@apollo/client/react"
import { useState, useCallback } from "react"
import {
  START_OBSERVATION,
  UPDATE_OBSERVATION,
  END_OBSERVATION,
  GET_OPEN_OBSERVATIONS,
} from "../graphql/operations/observationOperations"
import { DASHBOARD_QUERY } from "../graphql/operations/dashboardOperations"
import { GET_TRANSLATED_ANTECEDENTS } from "../graphql/operations/antecedentOperations"
import { GET_TRANSLATED_BEHAVIORS } from "../graphql/operations/behaviorOperations"
import { GET_TRANSLATED_CONSEQUENCES } from "../graphql/operations/consequenceOperations"
import type { DailyContext, Observation } from "../types/observation"

interface SelectableItem {
  id: string
  name: string
  description: string
}

interface AntecedentsData {
  translatedAntecedents: SelectableItem[]
}
interface BehaviorsData {
  translatedBehaviors: SelectableItem[]
}
interface ConsequencesData {
  translatedConsequences: SelectableItem[]
}
interface OpenObservationsData {
  observations: { nodes: Observation[] }
}

const defaultDailyContext: DailyContext = {
  hadBreakfast: false,
  hadLunch: false,
  hadDinner: false,
  hadSnack: false,
  sleptWell: false,
  hoursOfSleep: null,
}

export function useObservation() {
  const [observation, setObservation] = useState<Observation | null>(null)
  const [selectedAntecedents, setSelectedAntecedents] = useState<string[]>([])
  const [selectedBehaviors, setSelectedBehaviors] = useState<string[]>([])
  const [selectedConsequences, setSelectedConsequences] = useState<string[]>([])
  const [notes, setNotes] = useState("")
  const [dailyContext, setDailyContext] = useState<DailyContext>({ ...defaultDailyContext })

  const {
    data: antecedentsData,
    loading: antecedentsLoading,
  } = useQuery<AntecedentsData>(GET_TRANSLATED_ANTECEDENTS, { errorPolicy: "all" })

  const {
    data: behaviorsData,
    loading: behaviorsLoading,
  } = useQuery<BehaviorsData>(GET_TRANSLATED_BEHAVIORS, { errorPolicy: "all" })

  const {
    data: consequencesData,
    loading: consequencesLoading,
  } = useQuery<ConsequencesData>(GET_TRANSLATED_CONSEQUENCES, { errorPolicy: "all" })

  const [startMutation, { loading: starting }] = useMutation(START_OBSERVATION)
  const [updateMutation, { loading: updating }] = useMutation(UPDATE_OBSERVATION)
  const [endMutation, { loading: ending }] = useMutation(END_OBSERVATION)
  const [fetchOpenObservations, { loading: loadingOpen }] = useLazyQuery<OpenObservationsData>(GET_OPEN_OBSERVATIONS, {
    fetchPolicy: "network-only",
  })

  const antecedents = antecedentsData?.translatedAntecedents ?? []
  const behaviors = behaviorsData?.translatedBehaviors ?? []
  const consequences = consequencesData?.translatedConsequences ?? []
  const listsLoading = antecedentsLoading || behaviorsLoading || consequencesLoading

  const startObservation = useCallback(
    async (childId: string) => {
      const { data } = await startMutation({
        variables: { childId, dailyContext },
      })
      if (data?.startObservation) {
        setObservation(data.startObservation)
      }
      return data?.startObservation ?? null
    },
    [startMutation, dailyContext]
  )

  const continueObservation = useCallback(
    (obs: Observation) => {
      setObservation(obs)
      setSelectedAntecedents(obs.antecedents.map((a) => a.id))
      setSelectedBehaviors(obs.behaviors.map((b) => b.id))
      setSelectedConsequences(obs.consequences.map((c) => c.id))
      setNotes(obs.notes || "")
      if (obs.dailyContext) {
        setDailyContext(obs.dailyContext)
      }
    },
    []
  )

  const getOpenObservations = useCallback(
    async (childId: string): Promise<Observation[]> => {
      const { data } = await fetchOpenObservations({
        variables: { childId },
      })
      return data?.observations?.nodes ?? []
    },
    [fetchOpenObservations]
  )

  const saveAndEndObservation = useCallback(async () => {
    if (!observation) return null

    const { data: updateData } = await updateMutation({
      variables: {
        observationId: observation.id,
        antecedents: selectedAntecedents,
        behaviors: selectedBehaviors,
        consequences: selectedConsequences,
        notes,
        dailyContext,
      },
    })

    if (updateData?.updateObservation) {
      setObservation(updateData.updateObservation)
    }

    const { data: endData } = await endMutation({
      variables: { observationId: observation.id },
      refetchQueries: [{ query: DASHBOARD_QUERY }],
    })

    if (endData?.endObservation) {
      setObservation(endData.endObservation)
    }

    return endData?.endObservation ?? null
  }, [
    observation,
    selectedAntecedents,
    selectedBehaviors,
    selectedConsequences,
    notes,
    dailyContext,
    updateMutation,
    endMutation,
  ])

  const toggleAntecedent = useCallback((id: string) => {
    setSelectedAntecedents((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    )
  }, [])

  const toggleBehavior = useCallback((id: string) => {
    setSelectedBehaviors((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    )
  }, [])

  const toggleConsequence = useCallback((id: string) => {
    setSelectedConsequences((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    )
  }, [])

  const canEnd =
    selectedAntecedents.length > 0 &&
    selectedBehaviors.length > 0 &&
    selectedConsequences.length > 0

  const reset = useCallback(() => {
    setObservation(null)
    setSelectedAntecedents([])
    setSelectedBehaviors([])
    setSelectedConsequences([])
    setNotes("")
    setDailyContext({ ...defaultDailyContext })
  }, [])

  return {
    observation,
    antecedents,
    behaviors,
    consequences,
    selectedAntecedents,
    selectedBehaviors,
    selectedConsequences,
    notes,
    setNotes,
    dailyContext,
    setDailyContext,
    toggleAntecedent,
    toggleBehavior,
    toggleConsequence,
    startObservation,
    continueObservation,
    getOpenObservations,
    saveAndEndObservation,
    canEnd,
    reset,
    listsLoading,
    starting,
    updating,
    ending,
    loadingOpen,
    saving: updating || ending,
  }
}
