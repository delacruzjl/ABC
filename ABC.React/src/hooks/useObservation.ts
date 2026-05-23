import { useMutation, useQuery } from "@apollo/client/react"
import { useState, useCallback } from "react"
import {
  START_OBSERVATION,
  UPDATE_OBSERVATION,
  END_OBSERVATION,
} from "../graphql/operations/observationOperations"
import { GET_ANTECEDENTS } from "../graphql/operations/antecedentOperations"
import { GET_BEHAVIORS } from "../graphql/operations/behaviorOperations"
import { GET_CONSEQUENCES } from "../graphql/operations/consequenceOperations"
import type { Observation } from "../types/observation"

interface SelectableItem {
  id: string
  name: string
  description: string
}

interface AntecedentsData {
  antecedents: { nodes: SelectableItem[] }
}
interface BehaviorsData {
  behaviors: { nodes: SelectableItem[] }
}
interface ConsequencesData {
  consequences: { nodes: SelectableItem[] }
}

export function useObservation() {
  const [observation, setObservation] = useState<Observation | null>(null)
  const [selectedAntecedents, setSelectedAntecedents] = useState<string[]>([])
  const [selectedBehaviors, setSelectedBehaviors] = useState<string[]>([])
  const [selectedConsequences, setSelectedConsequences] = useState<string[]>([])
  const [notes, setNotes] = useState("")

  const {
    data: antecedentsData,
    loading: antecedentsLoading,
  } = useQuery<AntecedentsData>(GET_ANTECEDENTS)

  const {
    data: behaviorsData,
    loading: behaviorsLoading,
  } = useQuery<BehaviorsData>(GET_BEHAVIORS)

  const {
    data: consequencesData,
    loading: consequencesLoading,
  } = useQuery<ConsequencesData>(GET_CONSEQUENCES)

  const [startMutation, { loading: starting }] = useMutation(START_OBSERVATION)
  const [updateMutation, { loading: updating }] = useMutation(UPDATE_OBSERVATION)
  const [endMutation, { loading: ending }] = useMutation(END_OBSERVATION)

  const antecedents = antecedentsData?.antecedents?.nodes ?? []
  const behaviors = behaviorsData?.behaviors?.nodes ?? []
  const consequences = consequencesData?.consequences?.nodes ?? []
  const listsLoading = antecedentsLoading || behaviorsLoading || consequencesLoading

  const startObservation = useCallback(
    async (childId: string) => {
      const { data } = await startMutation({ variables: { childId } })
      if (data?.startObservation) {
        setObservation(data.startObservation)
      }
      return data?.startObservation ?? null
    },
    [startMutation]
  )

  const saveAndEndObservation = useCallback(async () => {
    if (!observation) return null

    // Submit the full selection set once, then end
    const { data: updateData } = await updateMutation({
      variables: {
        observationId: observation.id,
        antecedents: selectedAntecedents,
        behaviors: selectedBehaviors,
        consequences: selectedConsequences,
        notes,
      },
    })

    if (updateData?.updateObservation) {
      setObservation(updateData.updateObservation)
    }

    const { data: endData } = await endMutation({
      variables: { observationId: observation.id },
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
    toggleAntecedent,
    toggleBehavior,
    toggleConsequence,
    startObservation,
    saveAndEndObservation,
    canEnd,
    reset,
    listsLoading,
    starting,
    updating,
    ending,
    saving: updating || ending,
  }
}
