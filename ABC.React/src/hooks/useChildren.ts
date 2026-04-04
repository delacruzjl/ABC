import { useQuery, useMutation } from "@apollo/client/react"
import { useMemo } from "react"
import {
  GET_CHILDREN,
  CREATE_CHILD,
  UPDATE_CHILD,
  REMOVE_CHILD,
  GET_USERS,
  GET_CHILD_CONDITIONS,
} from "../graphql/operations/childOperations"
import type { Child, ChildCondition, UserInfo } from "../types/child"

interface ChildNode {
  id: string
  firstName: string
  lastName: string
  birthYear: number
  userId: string
  conditions: { id: string; name: string }[]
  observations: { id: string }[]
}

interface ChildrenQueryData {
  children: { nodes: ChildNode[] }
}

interface UsersQueryData {
  users: UserInfo[]
}

interface ChildConditionsQueryData {
  childConditions: { nodes: ChildCondition[] }
}

export function useChildren() {
  const { data, loading, error } = useQuery<ChildrenQueryData>(GET_CHILDREN)

  const children: Child[] = useMemo(
    () =>
      (data?.children?.nodes ?? []).map((node) => ({
        id: node.id,
        firstName: node.firstName,
        lastName: node.lastName,
        birthYear: node.birthYear,
        userId: node.userId,
        conditions: node.conditions ?? [],
        observationCount: node.observations?.length ?? 0,
      })),
    [data]
  )

  const [createChildMutation] = useMutation(CREATE_CHILD, {
    refetchQueries: [{ query: GET_CHILDREN }],
  })

  const [updateChildMutation] = useMutation(UPDATE_CHILD, {
    refetchQueries: [{ query: GET_CHILDREN }],
  })

  const [removeChildMutation] = useMutation(REMOVE_CHILD, {
    refetchQueries: [{ query: GET_CHILDREN }],
  })

  const createChild = (
    firstName: string,
    lastName: string,
    birthYear: number,
    conditions?: string[],
    userId?: string
  ) =>
    createChildMutation({
      variables: { firstName, lastName, birthYear, conditions, userId },
    })

  const updateChild = (
    childId: string,
    firstName: string,
    lastName: string,
    birthYear: number,
    userId?: string,
    conditions?: string[]
  ) =>
    updateChildMutation({
      variables: { childId, firstName, lastName, birthYear, userId, conditions },
    })

  const removeChild = (childId: string) =>
    removeChildMutation({ variables: { childId } })

  return { children, loading, error, createChild, updateChild, removeChild }
}

export function useUsers() {
  const { data, loading, error } = useQuery<UsersQueryData>(GET_USERS)
  const users: UserInfo[] = useMemo(() => data?.users ?? [], [data])
  return { users, loading, error }
}

export function useChildConditions() {
  const { data, loading, error } =
    useQuery<ChildConditionsQueryData>(GET_CHILD_CONDITIONS)
  const conditions: ChildCondition[] = useMemo(
    () => data?.childConditions?.nodes ?? [],
    [data]
  )
  return { conditions, loading, error }
}
