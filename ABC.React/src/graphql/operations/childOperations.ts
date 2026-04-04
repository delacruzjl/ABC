import { gql } from "@apollo/client"

export const GET_CHILDREN = gql`
  query GetChildren {
    children(first: 100) {
      nodes {
        id
        firstName
        lastName
        birthYear
        userId
        conditions {
          id
          name
        }
        observations {
          id
        }
      }
    }
  }
`

export const CREATE_CHILD = gql`
  mutation CreateChild(
    $firstName: String!
    $lastName: String!
    $birthYear: Int!
    $conditions: [String!]
    $userId: String
  ) {
    createChild(
      firstName: $firstName
      lastName: $lastName
      birthYear: $birthYear
      conditions: $conditions
      userId: $userId
    ) {
      id
      firstName
      lastName
      birthYear
      userId
    }
  }
`

export const UPDATE_CHILD = gql`
  mutation UpdateChild(
    $childId: UUID!
    $firstName: String!
    $lastName: String!
    $birthYear: Int!
    $userId: String
    $conditions: [String!]
  ) {
    updateChild(
      childId: $childId
      firstName: $firstName
      lastName: $lastName
      birthYear: $birthYear
      userId: $userId
      conditions: $conditions
    ) {
      id
      firstName
      lastName
      birthYear
      userId
      conditions {
        id
        name
      }
    }
  }
`

export const REMOVE_CHILD = gql`
  mutation RemoveChild($childId: UUID!) {
    removeChild(childId: $childId)
  }
`

export const GET_USERS = gql`
  query GetUsers {
    users {
      id
      email
      roles
    }
  }
`

export const GET_CHILD_CONDITIONS = gql`
  query GetChildConditions {
    childConditions(first: 100) {
      nodes {
        id
        name
      }
    }
  }
`
