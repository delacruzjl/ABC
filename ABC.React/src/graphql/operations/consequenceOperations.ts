import { gql } from "@apollo/client"

export const GET_CONSEQUENCES = gql`
  query GetConsequences {
    consequences {
      nodes {
        id
        name
        description
      }
    }
  }
`

export const GET_TRANSLATED_CONSEQUENCES = gql`
  query GetTranslatedConsequences {
    translatedConsequences {
      id
      name
      description
    }
  }
`

export const CREATE_CONSEQUENCE = gql`
  mutation CreateConsequence($name: String!, $description: String!) {
    createConsequence(name: $name, description: $description) {
      id
      name
      description
    }
  }
`

export const REMOVE_CONSEQUENCE = gql`
  mutation RemoveConsequence($consequenceId: UUID!) {
    removeConsequence(consequenceId: $consequenceId)
  }
`
