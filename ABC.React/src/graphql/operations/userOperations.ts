import { gql } from "@apollo/client"

export const DEACTIVATE_USER_MUTATION = gql`
  mutation DeactivateUser($userId: String!) {
    deactivateUser(userId: $userId) {
      id
      email
      roles
      isActive
      hasChildren
      hasObservations
    }
  }
`

export const REACTIVATE_USER_MUTATION = gql`
  mutation ReactivateUser($userId: String!) {
    reactivateUser(userId: $userId) {
      id
      email
      roles
      isActive
      hasChildren
      hasObservations
    }
  }
`

export const DELETE_USER_MUTATION = gql`
  mutation DeleteUser($userId: String!) {
    deleteUser(userId: $userId)
  }
`
