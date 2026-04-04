import { useQuery, useMutation } from "@apollo/client/react"
import {
  GET_BEHAVIORS,
  CREATE_BEHAVIOR,
  REMOVE_BEHAVIOR,
} from "../graphql/operations/behaviorOperations"
import type { Behavior } from "../types/behavior"

interface BehaviorsQueryData {
  behaviors: {
    nodes: Behavior[]
  }
}

export function useBehaviors() {
  const { data, loading, error } = useQuery<BehaviorsQueryData>(GET_BEHAVIORS)

  const [createMutation, { loading: creating }] = useMutation(
    CREATE_BEHAVIOR,
    { refetchQueries: [{ query: GET_BEHAVIORS }] }
  )

  const [removeMutation, { loading: removing }] = useMutation(
    REMOVE_BEHAVIOR,
    { refetchQueries: [{ query: GET_BEHAVIORS }] }
  )

  const behaviors = data?.behaviors?.nodes ?? []

  const createBehavior = (name: string, description: string) =>
    createMutation({ variables: { name, description } })

  const removeBehavior = (behaviorId: string) =>
    removeMutation({ variables: { behaviorId } })

  // Simulates update via delete + create (API has no update mutation)
  const updateBehavior = async (
    id: string,
    name: string,
    description: string
  ) => {
    await removeMutation({ variables: { behaviorId: id } })
    return createMutation({
      variables: { name, description },
      refetchQueries: [{ query: GET_BEHAVIORS }],
    })
  }

  return {
    behaviors,
    loading,
    error,
    creating,
    removing,
    createBehavior,
    removeBehavior,
    updateBehavior,
  }
}
