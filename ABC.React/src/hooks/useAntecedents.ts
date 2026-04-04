import { useQuery, useMutation } from "@apollo/client/react"
import {
  GET_ANTECEDENTS,
  CREATE_ANTECEDENT,
  REMOVE_ANTECEDENT,
} from "../graphql/operations/antecedentOperations"
import type { Antecedent } from "../types/antecedent"

interface AntecedentsQueryData {
  antecedents: {
    nodes: Antecedent[]
  }
}

export function useAntecedents() {
  const { data, loading, error } = useQuery<AntecedentsQueryData>(
    GET_ANTECEDENTS
  )

  const [createMutation, { loading: creating }] = useMutation(
    CREATE_ANTECEDENT,
    { refetchQueries: [{ query: GET_ANTECEDENTS }] }
  )

  const [removeMutation, { loading: removing }] = useMutation(
    REMOVE_ANTECEDENT,
    { refetchQueries: [{ query: GET_ANTECEDENTS }] }
  )

  const antecedents = data?.antecedents?.nodes ?? []

  const createAntecedent = (name: string, description: string) =>
    createMutation({ variables: { name, description } })

  const removeAntecedent = (antecedentId: string) =>
    removeMutation({ variables: { antecedentId } })

  // Simulates update via delete + create (API has no update mutation)
  const updateAntecedent = async (
    id: string,
    name: string,
    description: string
  ) => {
    await removeMutation({ variables: { antecedentId: id } })
    return createMutation({
      variables: { name, description },
      refetchQueries: [{ query: GET_ANTECEDENTS }],
    })
  }

  return {
    antecedents,
    loading,
    error,
    creating,
    removing,
    createAntecedent,
    removeAntecedent,
    updateAntecedent,
  }
}
