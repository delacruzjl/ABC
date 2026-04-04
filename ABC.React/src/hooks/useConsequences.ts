import { useQuery, useMutation } from "@apollo/client/react"
import {
  GET_CONSEQUENCES,
  CREATE_CONSEQUENCE,
  REMOVE_CONSEQUENCE,
} from "../graphql/operations/consequenceOperations"
import type { Consequence } from "../types/consequence"

interface ConsequencesQueryData {
  consequences: {
    nodes: Consequence[]
  }
}

export function useConsequences() {
  const { data, loading, error } = useQuery<ConsequencesQueryData>(
    GET_CONSEQUENCES
  )

  const [createMutation, { loading: creating }] = useMutation(
    CREATE_CONSEQUENCE,
    { refetchQueries: [{ query: GET_CONSEQUENCES }] }
  )

  const [removeMutation, { loading: removing }] = useMutation(
    REMOVE_CONSEQUENCE,
    { refetchQueries: [{ query: GET_CONSEQUENCES }] }
  )

  const consequences = data?.consequences?.nodes ?? []

  const createConsequence = (name: string, description: string) =>
    createMutation({ variables: { name, description } })

  const removeConsequence = (consequenceId: string) =>
    removeMutation({ variables: { consequenceId } })

  // Simulates update via delete + create (API has no update mutation)
  const updateConsequence = async (
    id: string,
    name: string,
    description: string
  ) => {
    await removeMutation({ variables: { consequenceId: id } })
    return createMutation({
      variables: { name, description },
      refetchQueries: [{ query: GET_CONSEQUENCES }],
    })
  }

  return {
    consequences,
    loading,
    error,
    creating,
    removing,
    createConsequence,
    removeConsequence,
    updateConsequence,
  }
}
